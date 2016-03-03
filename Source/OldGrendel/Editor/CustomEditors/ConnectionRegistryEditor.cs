using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ConnectionRegistry))]
public class ConnectionRegistryEditor : GrendelEditor<ConnectionRegistry>
{	
	private bool _drawAllConnections = false;
	private List<EditorObjectConnection> _scrubList = new List<EditorObjectConnection>();

	public override void OnInspectorGUI()
	{
        EditorGUIUtility.LookLikeControls();
		EditorGUILayout.LabelField("Connection Count: " + Target.Registry.Count);
		
		EditorGUILayout.Space();
		
		if (Target.Registry.Count <= 0) { return; }
		
		_drawAllConnections = GUILayout.Toggle(_drawAllConnections, "DRAW ALL CONNECTIONS", GUI.skin.button);
		
		if(GUILayout.Button("CLEAR CONNECTIONS"))
		{			
			ClearConnectionMenu();
		}		
		
		EditorGUILayout.Separator();
		
		EditorObject caller	= null;
		
		foreach(EditorObjectConnection connection in Target.Registry)
		{			
			//check for nulls
			if (connection.Caller == null || connection.Subject == null)
			{
				_scrubList.Add(connection);
				continue;
			}			
			
			if (caller != connection.Caller)
			{
				EditorGUI.indentLevel = 0;
				EditorGUILayout.LabelField(connection.Caller.name, EditorStyles.boldLabel);
				
				if(GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
				{
					DrawConnectionsForObject(caller);
				}
				
				caller = connection.Caller;
			}
			
			EditorGUI.indentLevel = 2;
			connection.SetColor();
			GUI.color = connection.MessageColor;			
			EditorGUILayout.LabelField(string.Format("{0} {1} {2} on {3}",connection.Caller, connection.Message, connection.Subject, connection.OnEvent.ToString()), GUI.skin.box);			
			GUI.color = Color.white;			
		}
		
		EditorUtility.SetDirty(Target);	
		
		if(_scrubList.Count > 0)
		{
			foreach(EditorObjectConnection connection in _scrubList)
			{
				Target.Registry.Remove(connection);
			}
		}
	}	
	
	private void OnSceneGUI()
	{		
		if (!_drawAllConnections)
		{
			return;
		}
		
		foreach(EditorObjectConnection connection in Target.Registry)
		{			
			DrawConnectionLine.DrawLine(connection.Caller.gameObject, connection.Subject.gameObject, connection.MessageColor);
		}
	}
	
	public static void DrawConnectionsForObject(EditorObject editorObject)
	{		
        if (ConnectionRegistry.DesignInstance == null)
        {
            return;
        }

        foreach(EditorObjectConnection connection in ConnectionRegistry.DesignInstance.Registry)
		{
			if (connection.Caller == editorObject)
			{				
				DrawConnectionLine.DrawLine(connection.Caller.gameObject, connection.Subject.gameObject, Selection.activeObject == editorObject.gameObject ? connection.MessageColor : GrendelColor.FlashingColor(connection.MessageColor, 4f));
			}
		}
	}
	
	public void ClearConnections(object obj)
	{
		Target.Registry.Clear();
	}
	
	public void DontClearConnections(object obj)
	{
		
	}
	
	public void ClearConnectionMenu()
	{		  
        GenericMenu menu = new GenericMenu ();
        
        menu.AddSeparator ("ARE YOU SURE YOU WANT TO CLEAR?");
		menu.AddSeparator ("");
		menu.AddItem(new GUIContent ("Clear"), false, ClearConnections, "");
        menu.AddItem(new GUIContent ("Don't Clear"), false, DontClearConnections, "");      
        
        menu.ShowAsContext ();
	}
}

