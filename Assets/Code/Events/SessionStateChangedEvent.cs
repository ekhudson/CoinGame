using UnityEngine;

public class SessionStateChangedEvent : EventBase
{
    public readonly SessionManager.SessionStates NewState;
    public readonly SessionManager.SessionStates PreviousState;

    public SessionStateChangedEvent(Object sender, SessionManager.SessionStates newState, SessionManager.SessionStates previousState) : base(Vector3.zero, sender)
    {
        NewState = newState;
        PreviousState = previousState;
    }
}
