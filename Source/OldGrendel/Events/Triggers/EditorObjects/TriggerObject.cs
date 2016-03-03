using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TriggerObject : EditorObject, IEditorObject 
{
	public delegate void OnTriggerEnterHandler(TriggerObject trigger, Collider intruder);
	public delegate void OnTriggerExitHandler(TriggerObject trigger, Collider intruder);
	
	[HideInInspector]public List<Collider> ObjectList = new List<Collider>();
	
	private List<Collider> _removeList = new List<Collider>();
	private float _scrubTimeInterval = 0.5f; //how often the list is scrubbed for nulls	
	
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
		
		StartCoroutine( ScrubList() );
	}	
	
	/// <summary>
	/// Scrubs the list for nulls
	/// </summary>	
	IEnumerator ScrubList()
	{
		while(true)
		{
			foreach(Collider other in ObjectList)
			{
				if (other != null)
				{
					EventManager.Instance.Post(this, new TriggerEventStay(this, other));
					continue;
				}
				else
				{
					_removeList.Add(other);
				}
			}
			
			foreach(Collider other in _removeList)
			{				
				ObjectList.Remove(other);				
			}		
			
			_removeList.Clear();
			
			yield return new WaitForSeconds(_scrubTimeInterval);
		}		
	}
	
	virtual public void OnTriggerEnter(Collider collider)
	{		
		EventManager.Instance.Post(this, new TriggerEventEnter(this, collider));
		
		ObjectList.Add(collider);		
	}
	
	virtual protected void OnTriggerExit(Collider collider)
	{	
		EventManager.Instance.Post(this, new TriggerEventExit(this, collider));
		
		ObjectList.Remove(collider);
	}
	
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		
		if (Application.isPlaying) { OnPlayGizmos(); } else { OnEditGizmos(); }	
		_gizmoName = "Gizmo_Trigger";
			
	}
	
	protected override void OnPlayGizmos()
	{
		base.OnPlayGizmos();
		
		if (ObjectList.Count > 0){ Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
			Gizmos.DrawWireSphere(transform.position, GetComponent<Collider>().bounds.extents.x);
		
		foreach(Collider other in ObjectList)
		{
			if (other == null){ continue; }
			Gizmos.color = Color.magenta;
			Gizmos.DrawLine(transform.position, other.transform.position);
		}
	}
	
	protected override void OnEditGizmos()
	{
		base.OnEditGizmos();
		
		if (_currentActiveObject == gameObject){ return; }
		
		Gizmos.color = Color.gray;
		Gizmos.DrawWireSphere(transform.position, GetComponent<Collider>().bounds.extents.x);
	}	
	
	void OnGUI()
	{
		OnSceneGUI();
	}
	
	
	
}
