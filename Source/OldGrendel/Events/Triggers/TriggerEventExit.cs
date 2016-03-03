using UnityEngine;
using System.Collections;

[System.Serializable]
public class TriggerEventExit : TriggerEventBase
{
	public TriggerEventExit(Object sender, Collider collider) : base (sender, collider)
	{	
		_collider = collider;
	}	
	
	public TriggerEventExit()
	{	
		_collider = null;
	}
}
