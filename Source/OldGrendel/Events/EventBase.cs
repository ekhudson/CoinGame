using UnityEngine;
using System;
   
[System.Serializable]
public class EventBase
{
    // Time at which the event occured.
    public readonly float Time;

    // Location at which the event occurred.
    public readonly Vector3 Place;	
	
	public readonly object Sender;
	
    // Create a new event.
    protected EventBase() : this(Vector3.zero, null)
    {
    }

    // Create a new event at a location in the world.
    protected EventBase(Vector3 place, object sender)
    {
       // Time = UnityEngine.Time.fixedTime; 
		Time = 0.2f;
        Place = place;
		Sender = sender;
    }
}
