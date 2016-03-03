using UnityEngine;
using System.Collections;

[System.Serializable]
public class TriggerEventEnter : TriggerEventBase
{
   	
	public TriggerEventEnter(Object sender, Collider collider) : base(sender, collider)
	{		
		_collider = collider;
	}
	
	public TriggerEventEnter()
	{		
		_collider = null;
	}
}
