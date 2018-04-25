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

        switch (newState)
        {
            case TurnStates.NotStarted:

            break;

            case TurnStates.TurnStarting:

                DoTurnStart();

            break;

            case TurnStates.Aiming:

            break;

            case TurnStates.Launched:

            break;

            case TurnStates.TurnEnding:
                
                DoTurnEnd();

            break;

            case TurnStates.TurnEnded:

            break;
        }

        TurnStates previousState = mCurrentState;
        mCurrentState = newState;

        EventManager.Instance.Post(new TurnStateChangedEvent(null, mCurrentState, previousState));
    }

    private void DoTurnStart()
    {
        SetState(TurnStates.Aiming);
    }

    private void DoTurnEnd()
    {
         SetState(TurnStates.TurnEnded);
    }
}
