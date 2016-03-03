//#define DEBUG
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EditorObject))]
[CanEditMultipleObjects] 
public class EditorObjectEditor<T> : Editor where T : class
{		
	protected EditorObject _target;
	protected int _editorWindowWidth = 896;
	protected int _editorWindowHeight = 384;
	
	protected bool _quickMenu = false;
	protected static EditorObject _currentHoveredObject;
	protected Camera _currentCamera;
	protected int _toolbarInt;
	protected Dictionary<string, int> _nameConflictCounts = new Dictionary<string, int>();	
	
	protected GenericMenu _messageMenu;
	
	private bool mShowOutgoingConnections = true;
	private bool mShowIncomingConnections = true;	
	
	//CONSTANTS
	private const int kCommentCharacterLimit = 512;
	private const int kTextFieldHeight = 64;
	private const int kTextFieldWidth = 256;
	private const int kConnectionScrollAreaWidth = 464;
	
	virtual public EditorObject Target
	{		
		get {return target as EditorObject;}		
	}
	
	virtual protected void OnEnable()
	{		
		if (!_target) { _target = target as EditorObject;}		
		CheckNameConflicts();				
		
		SetAllFiltersOn();
		
		if (Target.FirstEnable)
		{
			SetAllFiltersOn();
			Target.FirstEnable = false;
		}		
	}
	
	void OnDisable()
	{
		CheckNameConflicts();		
	}	
	
	private void SetAllFiltersOn()
	{
		Target.OutgoingEventFilters = new bool[Target.AssociatedEventsAsStrings.Length];
		
		for(int i = 0; i < Target.OutgoingEventFilters.Length; i++)
		{
			Target.OutgoingEventFilters[i] = true;
		}
	}
	
	void CheckNameConflicts()
	{
		_nameConflictCounts.Clear();
		
		EditorObject[] editorObjects = GameObject.FindObjectsOfType(typeof(EditorObject)) as EditorObject[];
		
		List<EditorObject> objectList = new List<EditorObject>(editorObjects);
		
		ObjectComparer comparer = new ObjectComparer();
		
		objectList.Sort(comparer);
		
		for (int i = 0; i < (objectList.Count); i++)
		{			
		
			if (objectList[i] == null) { continue; }
			
			objectList[i].NameConflict = false;	
			
			try
			{
				if (objectList[i].name == objectList[i + 1].name)
				{
					objectList[i].NameConflict = true;
					
					if (_nameConflictCounts.ContainsKey(objectList[i].name)){ _nameConflictCounts[objectList[i].name]++; }
					else{ _nameConflictCounts.Add(objectList[i].name, 1); }					
				
					continue;
				}
			}
			catch
			{
				
			}
			
			try
			{
				if (objectList[i].name == objectList[i - 1].name)
				{
					objectList[i].NameConflict = true;
						
					if (_nameConflictCounts.ContainsKey(objectList[i].name)){ _nameConflictCounts[objectList[i].name]++; }
					else{ _nameConflictCounts.Add(objectList[i].name, 1); }
							
					continue;
				}
			}
			catch
			{
				//we expect errors with this
			}
		}		
	}
	
	virtual protected void OnSceneGUI()
	{		
		
		_currentCamera = SceneView.currentDrawingSceneView.camera;		
	
		GetInput();				
		DrawSearchLine();	
    	
		ConnectionRegistryEditor.DrawConnectionsForObject(Target);
		
		if(_quickMenu){ DrawQuickMenu(); }	

	}

	public void CustomInspector()
	{
		SerializedObject sObject = new SerializedObject (_target);
		SerializedProperty comment = sObject.FindProperty("Comment");
		string nameForTest = _target.name;
		
		int nameConflicts = _nameConflictCounts.ContainsKey(_target.name) ? _nameConflictCounts[_target.name] : 0;
		Color nameColor = _target.NameConflict == true ? Color.red : Color.black;
				
		
		GUILayout.BeginVertical();	
		
			GUILayout.BeginHorizontal();				
				GUILayout.Label(new GUIContent(string.Format("Name: {0}", nameConflicts <= 0 ? "" : nameConflicts.ToString()),
								nameConflicts <= 0 ? "" : string.Format("There are {0} name conflicts.", nameConflicts.ToString())),
								GrendelCustomStyles.CustomElement(GUI.skin.label, Color.black, nameColor));
			
				GUI.skin.textField.focused.textColor = nameColor;
				_target.name = GUILayout.TextField(_target.name, GrendelCustomStyles.CustomElement(GUI.skin.textArea, Color.white, nameColor));	
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();			
				GUILayout.Label("Comment: ");			
				GUI.skin.textField.wordWrap = true;
				comment.stringValue = GUILayout.TextField(comment.stringValue, kCommentCharacterLimit, GUI.skin.textField, GUILayout.Height(64));		
			
			GUILayout.EndHorizontal();	
			sObject.ApplyModifiedProperties();		
			EditorGUIUtility.LookLikeControls();			
		
		GUILayout.EndVertical();
		
		
		if (_target.name != nameForTest || _nameConflictCounts == null)
		{
			CheckNameConflicts();
		}			
	}	
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		CustomInspector();
		
		Target.LookingForSubject = GUILayout.Toggle(_target.LookingForSubject, "Add", GrendelCustomStyles.CustomElement(GUI.skin.button, Color.green, Color.white, TextAnchor.MiddleCenter, FontStyle.Bold));			
		
		mShowOutgoingConnections = EditorGUILayout.Foldout(mShowOutgoingConnections, new GUIContent("Outgoing Connections", "Display the Outgoing Connections for this object"));
		
		if (mShowOutgoingConnections)
		{
			DrawFilters(Target.OutgoingEventFilters, Target.OutgoingMessageFilters);
			DrawConnectionBoxes();		
		}
		
		mShowIncomingConnections = EditorGUILayout.Foldout(mShowIncomingConnections, new GUIContent("Incoming Connections", "Display the Incoming Connections for this object"));
		
		if (mShowIncomingConnections)
		{
				
		}
		
		EditorUtility.SetDirty(_target);
		EditorUtility.SetDirty(EditorObjectManager.DesignInstance);
	}
	
	private void DrawFilters(bool[] eventFilters, bool[] actionFilters)
	{
			
		GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
				for(int i = 0; i < eventFilters.Length; i++)
				{
					eventFilters[i] = GUILayout.Toggle(eventFilters[i], Target.AssociatedEventsAsStrings[i]);
				}
			GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	private void DrawConnectionBoxes()
	{			
		//SerializedProperty infoScrollPos = serializedObject.FindProperty("InfoScrollPos");
		GUI.color = Color.gray;
		Target.InfoScrollPos = GUILayout.BeginScrollView(Target.InfoScrollPos, false, true, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.textArea);
		GUI.color = Color.white;
		//serializedObject.ApplyModifiedProperties();							
			
			if (ConnectionRegistry.DesignInstance.Registry.Count <= 0)
			{
				//do nothing
			}
			else
			{				
				for(int i = (ConnectionRegistry.DesignInstance.Registry.Count - 1); i >= 0; i--)
				{					
					EditorObjectConnection connection = ConnectionRegistry.DesignInstance.Registry[i];
				
					if (connection == null || connection.Caller != Target)
					{
						continue;
					}				
				
					DrawConnectionBox(connection.MessageColor, connection, Vector3.zero);						
				}				
			}	
		
		GUILayout.EndScrollView();
		
	}	
	
	void DrawConnectionBox(Color color, EditorObjectConnection editorObjectConnection, Vector3 origin)
	{				
		Event e = Event.current;
		
		GUI.color = Color.black;
		GUILayout.BeginHorizontal(GUI.skin.box);	
		GUI.color = Color.cyan;
		
		if (e.type == EventType.repaint)
		{
			if (editorObjectConnection.Subject.HighlightHighlight)
			{			
				GUI.color = Color.cyan;			
			}
			else
			{
				GUI.color = color;
			}
		}	
		
		SerializedObject connectionSerialized = new SerializedObject(editorObjectConnection);
		SerializedProperty eventSerialized = connectionSerialized.FindProperty("OnEvent");
		SerializedProperty messageSerialized = connectionSerialized.FindProperty("Message");

		
		if( GUILayout.Button("Del", GrendelCustomStyles.CustomElement(GUI.skin.button, color, Color.black, TextAnchor.MiddleCenter)))
		{					
		
			ConnectionRegistry.DesignInstance.Registry.Remove(editorObjectConnection);
			EditorUtility.SetDirty(ConnectionRegistry.DesignInstance);
		}
		
		eventSerialized.enumValueIndex = EditorGUILayout.Popup(eventSerialized.enumValueIndex, Target.AssociatedEventsAsStrings, EditorStyles.toolbarPopup);
		GUILayout.Space(4);
		messageSerialized.enumValueIndex = EditorGUILayout.Popup(messageSerialized.enumValueIndex, messageSerialized.enumNames, EditorStyles.toolbarPopup);
	
		if(GUILayout.Button(new GUIContent(editorObjectConnection.Subject.ToString(), "Click to select " + editorObjectConnection.Subject.ToString()), GrendelCustomStyles.CustomElement(GUI.skin.button, color, Color.black,TextAnchor.MiddleLeft)))
		{
			Selection.activeGameObject = editorObjectConnection.Subject.gameObject;	
		}

		connectionSerialized.ApplyModifiedProperties();
	
		GUILayout.EndHorizontal();
		
		Rect boxRect = GUILayoutUtility.GetLastRect();			
				
		if (e.type == EventType.Repaint && boxRect.Contains(e.mousePosition))
		{				
			editorObjectConnection.Subject.HighlightHighlight = true;			
		}
		else if (e.type == EventType.Repaint && !boxRect.Contains(e.mousePosition))
		{			
			editorObjectConnection.Subject.HighlightHighlight = false;
		}
		
		GUI.color = Color.white;
	}
	
	public void DrawConnectionHelper(Vector3 sourcePosition, Vector3 targetPosition)
	{		
		//reverse if we're drawing the backwards connection
		if (targetPosition == Target.transform.position)
		{			
			targetPosition = sourcePosition;
			sourcePosition = Target.transform.position;
		}
		
		float arrowSize = 0.5f;
		Vector3 direction = (sourcePosition - targetPosition).normalized;		
		Quaternion rotation = Quaternion.LookRotation(direction);
		rotation *= Quaternion.Euler(0, 180, 0);		
		Handles.ArrowCap(0, targetPosition + (( direction * arrowSize) / 0.5f), rotation, arrowSize);	
		Handles.DrawLine( sourcePosition,  targetPosition + (( direction * arrowSize) / 0.5f));
	}
	
	void DrawSearchLine()
	{			
		Event e = Event.current;
		Vector3 mousePos = e.mousePosition;
		mousePos.y = _currentCamera.pixelHeight - mousePos.y;	
		mousePos.z = 10f;
		//mousePos = _currentCamera.camera.ScreenToWorldPoint(mousePos);
		
		if (_target.LookingForSubject) 
		{ 
			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));			
			
			DrawConnectionLine.DrawLine(_target.transform.position, mousePos, Color.green);
		}		
	}
	
	void GetInput()
	{		
		Event e = Event.current;
		
		if (e.button == 0 && e.type == EventType.MouseDown && Target.LookingForSubject)			    
		{			
			
			ChooseObject(EditorObject.EditorObjectMessage.Activate);

		}
		else if (e.button == 1 && e.type == EventType.MouseDown)			    
		{		
			if(EditorObject.CurrentHoveredEditorObject != null && EditorObject.CurrentHoveredEditorObject != _target)
			{			
				DrawQuickMenu();
			}
		}
		
		if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Escape))
		{
			MadeSelection(-1); //cancel out the menu
			CloseOpenConnections();			
		}
		else if (e.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.C))
		{			
			ChooseObject(EditorObject.EditorObjectMessage.Activate);
		}
	}
	
	void DrawQuickMenu()
	{
		int count = 0;	
	
		List<string> messages = new List<string>(Enum.GetNames(typeof(EditorObject.EditorObjectMessage)));
		
		if (ConnectionRegistry.DesignInstance.ContainsConnection(EditorObject.CurrentHoveredEditorObject, Target))
		{
			messages.Add("REMOVE CONNECTION");
		}
		
        if( _messageMenu == null) { _messageMenu = new GenericMenu(); }		
       
		foreach(string message in messages)
		{
			_messageMenu.AddItem(new GUIContent(message), false, MadeSelection, count);
			count++;
		} 
		
        _messageMenu.ShowAsContext();		
	}
	
	void MadeSelection(object userData)
	{
		_quickMenu = false;		
		
		switch((int)userData)
		{
			case -1:			
			
			break;
		
			case 0:
			
				ChooseObject(EditorObject.EditorObjectMessage.None);
			
			break;
			
			case 1:
			
				ChooseObject(EditorObject.EditorObjectMessage.Activate);
			
			break;
			
			case 2:
			
				ChooseObject(EditorObject.EditorObjectMessage.Deactivate);
			
			break;
			
			case 3:
			
				ChooseObject(EditorObject.EditorObjectMessage.Toggle);
			
			break;
			
			case 4:
			
				ChooseObject(EditorObject.EditorObjectMessage.Enable);
			
			break;
			
			case 5:
			
				ChooseObject(EditorObject.EditorObjectMessage.Disable);
			
			break;
			
			case 6:
			
				ConnectionRegistry.DesignInstance.Registry.Remove(ConnectionRegistry.DesignInstance.ContainsConnection(EditorObject.CurrentHoveredEditorObject, Target));
				EditorUtility.SetDirty(ConnectionRegistry.DesignInstance);		
				new SerializedObject(ConnectionRegistry.DesignInstance).SetIsDifferentCacheDirty();
			
			break;
			
			default:
			
			break;
		}	}
	
	void CloseOpenConnections()
	{		
		_target.LookingForSubject = false;
		_quickMenu = false;
	}
	
	void ChooseObject(EditorObject.EditorObjectMessage message)
	{		
		if (EditorObject.CurrentHoveredEditorObject == null)
		{			
			return;
		}	
				
		ConnectionRegistry.DesignInstance.AddConnection(EditorObject.CurrentHoveredEditorObject, _target, message, Target.AssociatedEvents[0]);
							
		EditorUtility.SetDirty(ConnectionRegistry.DesignInstance);		
		new SerializedObject(ConnectionRegistry.DesignInstance).SetIsDifferentCacheDirty();
		
		EditorObject.CurrentHoveredEditorObject = null;
		_target.LookingForSubject = false;		
			
	}	

	[DrawGizmo (GizmoType.NotInSelectionHierarchy | GizmoType.Selected | GizmoType.Pickable)]
	static void DrawGizmos(EditorObject eo, GizmoType gizmoType)
	{	
		if (!Application.isPlaying)
		{			
			if (SceneView.currentDrawingSceneView == null || SceneView.currentDrawingSceneView.camera == null)
			{
				return;
			}		
			 
			Camera currentCamera = SceneView.currentDrawingSceneView.camera;
			
			if (eo.ActivateHighlight) { Gizmos.DrawIcon(eo.transform.position, "Gizmo_Activate_Ring"); }
			if (eo.DeactivateHighlight) { Gizmos.DrawIcon(eo.transform.position, "Gizmo_Deactivate_Ring"); }
			if (eo.ToggleHighlight) { Gizmos.DrawIcon(eo.transform.position, "Gizmo_Toggle_Ring"); }			
							
			eo.ActivateHighlight = false;
			eo.DeactivateHighlight = false;
			eo.ToggleHighlight = false;					
			
			Vector3 centre = eo.transform.position;
			Vector3 right = (centre + (currentCamera.transform.right * 0.4f));
			Vector3 centreConverted = currentCamera.WorldToScreenPoint(centre);
			Vector3 rightConverted = currentCamera.WorldToScreenPoint(right);
			float dist = Mathf.Abs(centreConverted.x - rightConverted.x);
			
			centreConverted.y = currentCamera.pixelHeight - centreConverted.y;			
			
			Rect labelRect = new Rect(centreConverted.x - dist, centreConverted.y - dist, dist * 2, dist * 2);				
			
			if (Selection.activeGameObject != eo.gameObject)
			{				
				EditorGUIUtility.AddCursorRect(labelRect, MouseCursor.Link);				
				
				if (HandleUtility.DistanceToCircle(eo.transform.position, 0.5f) <= 0.5f)
				{					
					eo.HighlightHighlight = true;	
					
					ConnectionRegistryEditor.DrawConnectionsForObject(eo);
											
//					foreach(EditorObjectConnection connection in ConnectionRegistry.DesignInstance.Registry)
//					{
//					
//						if (connection.Caller != eo)
//						{
//							continue;
//						}
//						else if (connection.Subject.HighlightHighlight)
//						{
//							Handles.color = Color.cyan;
//							Handles.DrawLine(eo.transform.position, connection.Subject.transform.position);
//							continue;
//						}	
						
//						Handles.color = GrendelColor.FlashingColor(connection.MessageColorDark, 4f);			
						
//							switch(connection.Message)
//							{
//								case EditorObject.EditorObjectMessage.Activate:				
//								
//								Handles.DrawLine(eo.transform.position, connection.Subject.transform.position);										
//								
//								break;
//								
//								case EditorObject.EditorObjectMessage.Deactivate:				
//								
//								Handles.DrawLine(eo.transform.position, connection.Subject.transform.position);									
//								
//								break;
//								
//								case EditorObject.EditorObjectMessage.Toggle:				
//								
//								Handles.DrawLine(eo.transform.position, connection.Subject.transform.position);								
//								
//								break;
//							
//								default:
//							
//								break;
//							}						
//						}				
					
					Gizmos.DrawIcon(eo.transform.position, "Gizmo_Cyan_Ring"); 
					Gizmos.DrawIcon(eo.transform.position, "Gizmo_Fill");
					EditorObject.CurrentHoveredEditorObject = eo;
				}
				else
				{
					if (EditorObject.CurrentHoveredEditorObject == eo)
					{
						EditorObject.CurrentHoveredEditorObject = null;
					}											
				}
				
				Vector3 pos = eo.transform.position;
				//pos += (currentCamera.transform.right) * 0.5f;
				//pos += (currentCamera.transform.up) * 0.15f;
                //pos += (currentCamera.transform.forward) * 0.15f;

                if( Vector3.Dot(currentCamera.transform.forward, eo.transform.position - currentCamera.transform.position) > 0)
                {
                    Handles.color = eo.NameConflict == true ? Color.red : Color.white;
    						
    				if (GrendelEditorPreferences.DrawEditorObjectLabels)
    				{
    					Handles.Label(pos, eo.name, GrendelCustomStyles.CustomElement(GUI.skin.label, Color.white, Handles.color));
    				}
    								
    				Handles.color = Color.white;
                }
			
			}
			else
			{			
				Gizmos.DrawIcon(eo.transform.position, "Gizmo_Selected");
				Gizmos.DrawIcon(eo.transform.position, "Gizmo_White_Ring");					
			}		
			
			if (eo.HighlightHighlight)
			{
				Gizmos.DrawIcon(eo.transform.position, "Gizmo_Cyan_Ring");				
			}
			
			eo.HighlightHighlight = false;
		}//end Application.isPlaying check		
		
		Gizmos.DrawIcon(eo.transform.position, eo.GizmoName);		
		
		try
		{
			if (SceneView.mouseOverWindow.GetInstanceID() == SceneView.currentDrawingSceneView.GetInstanceID()) 
			{				
				SceneView.RepaintAll(); 
			}
		}
		catch
		{
			//mouse is out of scene view, no biggie
		}
	}
}//end class

