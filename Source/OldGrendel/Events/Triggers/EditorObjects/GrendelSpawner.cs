using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class GrendelSpawner : EditorObject, IEditorObject
{	
	public List<GameObject> EntitiesToSpawn = new List<GameObject>();
	[HideInInspector]public Color MyColor = Color.white;
	
	public override EventTransceiver.Events[] AssociatedEvents
	{
		get
		{
			if(_associatedEvents == null || _associatedEvents.Length <= 0)
			{
				_associatedEvents = new EventTransceiver.Events[]{ EventTransceiver.Events.TriggerEventEnter,
									  							   EventTransceiver.Events.TriggerEventExit,
								     					           EventTransceiver.Events.TriggerEventStay
																 };
			}
			
			return _associatedEvents;
		}
	}

	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
		
	}		
	
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		
		if (Application.isPlaying) { OnPlayGizmos(); } else { OnEditGizmos(); }	
		_gizmoName = "Gizmo_Spawner";
			
	}
	
	public override void OnActivate(object caller, EventBase evt) //called when the editor object is activated
	{		
		base.OnActivate(caller, evt);
		
		if (!WaitingForCallerAndMessage(caller as EditorObject, EditorObject.EditorObjectMessage.Activate))
		{
			return;
		}
		
		foreach(GameObject entityToSpawn in EntitiesToSpawn)
		{
			GameObject.Instantiate(entityToSpawn, transform.position, Quaternion.identity); 
		}
	}
	
	public override void OnDeactivate(object caller, EventBase evt) //called when the editor object is deactivated
	{
		base.OnDeactivate(caller, evt);
	}
	
	public override void OnToggle(object caller, EventBase evt) //called when the editor object is toggled
	{
		base.OnToggle(caller, evt);
	}
	
	public override void OnEnabled(object caller, EventBase evt) //called when the editor object is deactivated
	{
		base.OnEnabled(caller, evt);
	}
	
	public override void OnDisabled(object caller, EventBase evt) //called when the editor object is toggled
	{
		base.OnDisabled(caller, evt);
	}
}

