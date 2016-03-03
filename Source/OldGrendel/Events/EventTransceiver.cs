using UnityEngine;
using System.Collections;

[System.Serializable]
public class EventTransceiver 
{
	public enum Events
	{
		TriggerEventEnter,
		TriggerEventExit,
		TriggerEventStay,
	}
	
	public static EventBase LookupEvent(Events evt)
	{
		
		EventBase returnEvent; 
		
		switch(evt)
		{
			case Events.TriggerEventEnter:
			
				returnEvent = new TriggerEventEnter();
			
			break;
			
			case Events.TriggerEventExit:
			
				returnEvent = new TriggerEventExit();
			
			break;
			
			case Events.TriggerEventStay:
			
				returnEvent = new TriggerEventStay();
			
			break;
			
			default:
			
				returnEvent = null;
			
			break;			
		}
		
		return returnEvent;
	}
	
	
}
