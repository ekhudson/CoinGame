using UnityEngine;
using System.Collections;

public class GameStateEvent : EventBase 
{
	public readonly GameManager.GameStates.STATES NewState;
	public readonly GameManager.GameStates.STATES OldState;
	
	
	public GameStateEvent(GameManager.GameStates.STATES newState, GameManager.GameStates.STATES oldState, object sender) : base (Vector3.zero, sender)
	{
		NewState = newState;
		OldState = oldState;
	}
	
	public GameStateEvent() : base (Vector3.zero, null)
	{
		
	}
}
