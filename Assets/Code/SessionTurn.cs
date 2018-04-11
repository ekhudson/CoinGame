using UnityEngine;

[System.Serializable]
public class SessionTurn
{
    public enum TurnStates
    {
        NotStarted,
        TurnStarting,
        Aiming,
        Launched,
        TurnEnding,
        TurnEnded,
    }

    private TurnStates mCurrentState = TurnStates.NotStarted;

    public TurnStates CurrentState
    {
        get
        {
            return mCurrentState;
        }
    }

    public void SetState(TurnStates newState)
    {
        if (newState == mCurrentState)
        {
            return;
        }

        TurnStates previousState = mCurrentState;
        mCurrentState = newState;

        EventManager.Instance.Post(new TurnStateChangedEvent(null, mCurrentState, previousState));
    }
}
