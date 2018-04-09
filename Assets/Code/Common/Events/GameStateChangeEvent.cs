using UnityEngine;
using System.Collections;

public class GameStateChangeEvent : EventBase
{
    public readonly GameManager.GameStates NewState;
    public readonly GameManager.GameStates PreviousState;

    public GameStateChangeEvent(Object sender, GameManager.GameStates newState, GameManager.GameStates previousState) : base(Vector3.zero, sender)
    {
        NewState = newState;
        PreviousState = previousState;
    }
}
