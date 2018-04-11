using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSessionData
{
    public Player Player;
    public List<CoinScript> CoinsCollected;
}

public class SessionManager : Singleton<SessionManager>
{
    public enum SessionStates
    {
        Starting,
        InProgress,
        Ending,
        OutOfSession,
    }

    private List<Player> mPlayerList;

    private int mCurrentPlayerIndex = 0;
    
    private SessionStates mCurrentState = SessionStates.OutOfSession;
    private SessionTurn mCurrentTurn = new SessionTurn();

    public List<Player> PlayerList
    {
        get
        {
            return mPlayerList;
        }
    }

    private void SetState(SessionStates newState)
    {
        if (newState == mCurrentState)
        {
            return;
        }

        switch(newState)
        {
            case SessionStates.OutOfSession:

            break;

            case SessionStates.Starting:

                StartSession();

            break;

            case SessionStates.InProgress:

            break;

            case SessionStates.Ending:

                EndSession();

            break;
        }

        SessionStates previousState = mCurrentState;
        mCurrentState = newState;

        EventManager.Instance.Post(new SessionStateChangedEvent(this, mCurrentState, previousState));
    }

    private void StartSession()
    {
        mCurrentPlayerIndex = Random.Range(0, mPlayerList.Count);
        StartCurrentTurn();
    }

    private void EndSession()
    {
        SetState(SessionStates.Ending);
    }

    private void StartCurrentTurn()
    {
        mCurrentTurn.SetState(SessionTurn.TurnStates.TurnStarting);
    }

    private void EndCurrentTurn()
    {
        mCurrentTurn.SetState(SessionTurn.TurnStates.TurnEnding);
    }
}
