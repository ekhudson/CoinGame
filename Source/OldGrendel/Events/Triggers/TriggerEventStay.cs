using UnityEngine;
using System.Collections;

[System.Serializable]
public class TriggerEventStay : TriggerEventBase 
{	
	public TriggerEventStay(Object sender, Collider collider) : base(sender, collider)
	{		
		_collider = collider;
	}
	
	public TriggerEventStay()
	{	
		_collider = null;
	}
}
