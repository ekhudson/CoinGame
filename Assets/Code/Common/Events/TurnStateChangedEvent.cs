using UnityEngine;

public class TurnStateChangedEvent : EventBase
{
    public readonly SessionTurn.TurnStates NewState;
    public readonly SessionTurn.TurnStates PreviousState;

    public TurnStateChangedEvent(Object sender, SessionTurn.TurnStates newState, SessionTurn.TurnStates previousState) : base(Vector3.zero, sender)
    {
        NewState = newState;
        PreviousState = previousState;
    }
}
