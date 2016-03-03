using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class ConnectionRegistry : Singleton<ConnectionRegistry>
{	
	[SerializeField]private List<EditorObjectConnection> _registry;		
	private EditorObjectConnectionComparer _comparer = new EditorObjectConnectionComparer();
	private static ConnectionRegistry _designInstance;

	protected override void Awake()
	{		
		base.Awake();		
	}
	
	protected void Start()
	{
		BuildConnections();
	}
		
	public void BuildConnections()
	{
		if (_registry.Count <= 0)
		{
			Console.Instance.OutputToConsole(string.Format("{0}: There were no connections to build.", this.ToString()), Console.Instance.Style_Admin);
			return;
		}
		
		Console.Instance.OutputToConsole(string.Format("{0}: Building {1} connections.",this.ToString(), _registry.Count), Console.Instance.Style_Admin);
		
		foreach(EditorObjectConnection connection in _registry)
		{			
			connection.Subject.AddCaller(connection);
			
			switch(connection.Message)
			{
				
				case EditorObject.EditorObjectMessage.Activate:
					
					EventManager.Instance.AddHandler(EventTransceiver.LookupEvent(connection.OnEvent).GetType(), connection.Subject.OnActivate);
					
				break;
					
				case EditorObject.EditorObjectMessage.Deactivate:
					
					EventManager.Instance.AddHandler(EventTransceiver.LookupEvent(connection.OnEvent).GetType(), connection.Subject.OnDeactivate);
					
				break;
					
				case EditorObject.EditorObjectMessage.Toggle:
					
					EventManager.Instance.AddHandler(EventTransceiver.LookupEvent(connection.OnEvent).GetType(),connection.Subject.OnToggle);
					
				break;
				
				case EditorObject.EditorObjectMessage.Enable:
					
					EventManager.Instance.AddHandler(EventTransceiver.LookupEvent(connection.OnEvent).GetType(),connection.Subject.OnEnabled);
					
				break;
				
				case EditorObject.EditorObjectMessage.Disable:
					
					EventManager.Instance.AddHandler(EventTransceiver.LookupEvent(connection.OnEvent).GetType(),connection.Subject.OnDisabled);
					
				break;
				
				default:
				
				break;				
			}			
		}
		
		Console.Instance.OutputToConsole(string.Format("{0}: Connections established.",this.ToString()), Console.Instance.Style_Admin);
	}
	
	public void RemoveBuiltConnections()
	{
		if (_registry.Count <= 0) { return; }
		
		Console.Instance.OutputToConsole(string.Format("{0}: Removing {1} connections that were built in the scene.",this.ToString(), _registry.Count), Console.Instance.Style_Admin);
		
		foreach(EditorObjectConnection connection in _registry)
		{			
			switch(connection.Message)
			{
				
				case EditorObject.EditorObjectMessage.Activate:
					
					EventManager.Instance.RemoveHandler(EventTransceiver.LookupEvent(connection.OnEvent).GetType(), connection.Subject.OnActivate);
					
				break;
					
				case EditorObject.EditorObjectMessage.Deactivate:
					
					EventManager.Instance.RemoveHandler(EventTransceiver.LookupEvent(connection.OnEvent).GetType(), connection.Subject.OnDeactivate);
					
				break;
					
				case EditorObject.EditorObjectMessage.Toggle:
					
					EventManager.Instance.RemoveHandler(EventTransceiver.LookupEvent(connection.OnEvent).GetType(),connection.Subject.OnToggle);
					
				break;
				
				case EditorObject.EditorObjectMessage.Enable:
					
					EventManager.Instance.RemoveHandler(EventTransceiver.LookupEvent(connection.OnEvent).GetType(),connection.Subject.OnEnabled);
					
				break;
				
				case EditorObject.EditorObjectMessage.Disable:
					
					EventManager.Instance.RemoveHandler(EventTransceiver.LookupEvent(connection.OnEvent).GetType(),connection.Subject.OnDisabled);
					
				break;
				
				default:
				
				break;				
			}
		}
	}
	
	public static ConnectionRegistry DesignInstance
	{
		get
		{
			if(_designInstance == null)
			{
				//_designInstance = GameObject.Find("ConnectionRegistry").GetComponent<ConnectionRegistry>();
				_designInstance = (ConnectionRegistry)FindObjectOfType(typeof(ConnectionRegistry));
			}
			
			return _designInstance;
		}
	}
	
	public List<EditorObjectConnection> Registry
	{
		get
		{			
			if (_registry == null)
			{				
				_registry = new List<EditorObjectConnection>();
			}
			
			return _registry;			
		}
		set
		{
			_registry = value;
			
		}		
	}
	
	public EditorObjectConnection ContainsConnection(EditorObject subject, EditorObject caller)
	{
		EditorObjectConnection testConnection = null;		
		
		foreach(EditorObjectConnection connection in Registry)
		{
			if(connection.Subject == subject && connection.Caller == caller)
			{					
				testConnection = connection;			
			}			
		}
		
		return testConnection;			
	}
	
	public void AddConnection(EditorObject subject, EditorObject caller, EditorObject.EditorObjectMessage message, EventTransceiver.Events onEvent)
	{					
		EditorObjectConnection connection = ContainsConnection(subject, caller);
		
		if (connection == null)
		{
		
			EditorObjectConnection newConnection = EditorObjectConnection.CreateInstance<EditorObjectConnection>();
				
			newConnection.Message = message;
			newConnection.Caller = caller;
			newConnection.Subject = subject;
			newConnection.OnEvent = onEvent;									
						
			Registry.Add(newConnection);
			Registry.Sort(_comparer);
		}
		else
		{
			Debug.LogWarning(string.Format("Attempted to add a connection between {0} and {1}, but a connection already exists. Setting new message instead.", subject.ToString(), caller.ToString()), this); 
			connection.Message = message;
		}
	}
	
	public void RemoveConnection(EditorObject subject, EditorObject caller)
	{
		EditorObjectConnection connectionToRemove = ContainsConnection(subject, caller);
		
		if (connectionToRemove != null)
		{			
			Registry.Remove(connectionToRemove);
		}
		else
		{
			Debug.LogWarning(string.Format("Attempted to remove a connection between {0} and {1}, but a connection does not exist", subject.ToString(), caller.ToString()), this); 
		}
	}
	
	class EditorObjectConnectionComparer : IComparer<EditorObjectConnection>
    {
        public int Compare(EditorObjectConnection x, EditorObjectConnection y)
        {
            return x.Caller.name.CompareTo(y.Caller.name);
        }
    }
	
	private void OnDestroy()
	{
		RemoveBuiltConnections();
	}
}

