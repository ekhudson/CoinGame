using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class EditorObject : BaseObject, IEditorObject
{
	#region PUBLIC (STATIC)
	public static EditorObject CurrentHoveredEditorObject;
	#endregion
	
	#region PUBLIC
	//public bool DebugMode = true;
	public int NumberOfUses = -1; //-1 == infinite
	
	[System.Serializable]public enum EditorObjectMessage
	{
		None,
		Activate, //turn on the editor object
		Deactivate, //turn off the editor object
		Toggle, //toggle the state of the editor object
		Enable, //enable a disabled editor object
		Disable //disable an enabled editor object		
	}
	
	public enum EDITOROBJECTSTATES 
	{
		ACTIVE, //has been activated and is running
		INACTIVE, //not running and awaiting activation
		DISABLED //will not receive any messages from other editor objects
	};
	#endregion
	
	#region PUBLIC (HIDDEN)
	[HideInInspector]public bool FirstEnable = true;
	[HideInInspector]public bool[] OutgoingEventFilters;
	[HideInInspector]public bool[] OutgoingMessageFilters;
	[HideInInspector]public bool[] IncomingMessageFilters;	
	[HideInInspector]public string Comment = "";
	[HideInInspector]public bool LookingForSubject = false;
	[HideInInspector]public bool ActivateHighlight = false; //the highlight to show when in editor when this object is going to be activated by another		
	[HideInInspector]public bool DeactivateHighlight = false; //the highlight to show when in editor when this object is going to be deactivated by another	
	[HideInInspector]public bool ToggleHighlight = false; //the highlight to show when in editor when this object is going to be toggled by another
	[HideInInspector]public bool HighlightHighlight = false; //the highlight to show when in editor when this object highlight via the info panel
	[HideInInspector]public Vector2 InfoScrollPos;
	#endregion
	
	#region PROTECTED
	//protected Transform _transform;
	//protected GameObject _gameObject;	
	protected int _labelWidth = 128;	
	protected GameObject _currentActiveObject;		
	protected Camera _cameraToUse;
	protected string _gizmoName = "";
	protected bool _nameConflict = false; //used by the editor to flag this object if it has a name conflict with another object
	protected EventTransceiver.Events[] _associatedEvents;
	protected EDITOROBJECTSTATES _state = EDITOROBJECTSTATES.INACTIVE;
	#endregion
	
	#region PRIVATE
	private bool _clearConnections = false;
	private string[] _associatedEventsAsStrings;
	//this is the registry that stores which editor objects this object is expecting to hear from
	private Dictionary<EditorObject, EditorObject.EditorObjectMessage> _callerRegistry = new Dictionary<EditorObject, EditorObject.EditorObjectMessage>();
	#endregion	
		
	#region ACCESSORS	
	public EDITOROBJECTSTATES State
	{
		get {return _state;}
		set {_state = value;}
	}
	
	public bool NameConflict
	{
		get {return _nameConflict;}
		set {_nameConflict = value;}
	}
	
	public string GizmoName
	{
		get {return _gizmoName;}		
	}
	
	//returns events associated with this object
	virtual public EventTransceiver.Events[] AssociatedEvents
	{
		get {return null;}
	}
	
	//returns the events associated with this objects as an array of string (for use in menus etc.)
	virtual public string[] AssociatedEventsAsStrings
	{
		get
		{ 
			
			if(_associatedEventsAsStrings == null || _associatedEventsAsStrings.Length <= 0)
			{
				_associatedEventsAsStrings = new string[AssociatedEvents.Length];
				for(int i = 0; i < AssociatedEvents.Length; i++)
				{
					_associatedEventsAsStrings[i] = AssociatedEvents[i].ToString();
				}
			}
			
			return _associatedEventsAsStrings;	
		}
	}
	
	public EditorObject() : base()
	{
		_clearConnections = true;				
	}
	
	protected override void Awake ()
	{
		base.Awake();
	}
	
	// Use this for initialization
	protected override void Start () 
	{
	
	}
	
	// Update is called once per frame
	virtual protected void Update () 
	{
	
	}	
	
	virtual public void OnSceneGUI()
	{		
		
	}
	#endregion
	
	virtual protected void OnDrawGizmos()
	{	
		switch(_state)
		{
			case EDITOROBJECTSTATES.ACTIVE:
			
				Gizmos.DrawIcon(transform.position, "Gizmo_Active");
			
			break;
			
			case EDITOROBJECTSTATES.INACTIVE:
			
				Gizmos.DrawIcon(transform.position, "Gizmo_Inactive");
			
			break;
			
			case EDITOROBJECTSTATES.DISABLED:
			
				Gizmos.DrawIcon(transform.position, "Gizmo_Disabled");
			
			break;
		}
		
		if (_clearConnections)
		{
			//Connections.Clear();
			//_clearConnections = false;
		}
		
		//OnEditGizmos();
	}
	
	virtual protected void OnPlayGizmos()
	{
		
	}
	
	virtual protected void OnEditGizmos()
	{
		
	}
	
	public void SetState(EDITOROBJECTSTATES state)
	{
		switch (state)
		{
			
			case EDITOROBJECTSTATES.ACTIVE:
				//stuff to do when going active
			break;
			
			case EDITOROBJECTSTATES.INACTIVE:
				//stuff to do when going inactive
			break;
			
			case EDITOROBJECTSTATES.DISABLED:
				//stuff to do when going disabled
			break;	
		}
	}
	
	//This EditorObject has been Activated
	virtual public void OnActivate(object caller, EventBase evt)
	{		
		switch(_state)
		{
			case EDITOROBJECTSTATES.ACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.INACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.DISABLED:
			
			break;
		}
	}
	
	//This EditorObject has been Deactivated
	virtual public void OnDeactivate(object caller, EventBase evt)
	{		
		switch(_state)
		{
			case EDITOROBJECTSTATES.ACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.INACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.DISABLED:
			
			break;
		}		
	}
	
	//This EditorObject has been Toggled
	virtual public void OnToggle(object caller, EventBase evt)
	{
		switch(_state)
		{
			case EDITOROBJECTSTATES.ACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.INACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.DISABLED:
			
			break;
		}		
	}
	
	virtual public void OnEnabled(object caller, EventBase evt)
	{
		switch(_state)
		{
			case EDITOROBJECTSTATES.ACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.INACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.DISABLED:
			
			break;
		}		
	}
	
	virtual public void OnDisabled(object caller, EventBase evt)
	{
		switch(_state)
		{
			case EDITOROBJECTSTATES.ACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.INACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.DISABLED:
			
			break;
		}		
	}	

	//returns false if there are no more uses left
	private bool DecrementUses()
	{
		if (NumberOfUses > 0)
		{
			if (NumberOfUses == -1)
			{
				//do not decrement
				return true;
			}
			else
			{
				NumberOfUses--; //do decrement
				return true;
			}
		}
		else
		{
			return false; //no uses left
		}
	}
	
	public void AddCaller(EditorObjectConnection connection)
	{
		if (_callerRegistry.ContainsKey(connection.Caller))
		{
			_callerRegistry[connection.Caller] = connection.Message;
		}
		else
		{
			_callerRegistry.Add(connection.Caller, connection.Message);
		}
	}
	
	public void RemoveCaller(EditorObject caller)
	{
		if (_callerRegistry.ContainsKey(caller))
		{
			_callerRegistry.Remove(caller);
		}
	}
	
	public bool WaitingForCallerAndMessage(EditorObject caller, EditorObject.EditorObjectMessage message)
	{
		if (_callerRegistry.ContainsKey(caller) && _callerRegistry[caller] == message)
		{
			return true;
		}
		
		return false;		
	}
}//end class

