using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EditorObjectManager : Singleton<EditorObjectManager> 
{	
	private Dictionary<EditorObject, List<EditorObjectConnection>> _connectionRegistry = new Dictionary<EditorObject, List<EditorObjectConnection>>();	
	
	public Dictionary<EditorObject, List<EditorObjectConnection>> ConnectionRegistry
	{
		get { return _connectionRegistry; }
	}
	
	public static EditorObjectManager DesignInstance
	{
		get
		{
			//return GameObject.Find("GameManager").GetComponent<EditorObjectManager>();
			return (EditorObjectManager)FindObjectOfType(typeof(EditorObjectManager));
		}
	}
	
	public bool ContainsConnection(EditorObject subject, EditorObject caller)
	{
		bool connectionExists = false;
		
		if (_connectionRegistry.ContainsKey(caller))
		{
			foreach(EditorObjectConnection connection in _connectionRegistry[caller])
			{
				if (connection.Subject == subject)
				{
					connectionExists = true;
				}			
			}
		}
		
		return connectionExists;			
	}
}

