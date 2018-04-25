using System.Collections.Generic;
using UnityEngine;

public class SessionManager : Singleton<SessionManager>
{
    public enum SessionStates
    {
        Starting,
        InProgress,
        Ending,
        OutOfSession,
    }

    private int mCurrentPlayerIndex = 0;
    
    private SessionStates mCurrentState = SessionStates.OutOfSession;
    private SessionTurn mCurrentTurn = new SessionTurn();

    public int CurrentPlayerIndex
    {
        get
        {
            return mCurrentPlayerIndex;
        }
    }

    public SessionStates CurrentSessionState
    {
        get
        {
            return mCurrentState;
        }
    }

    public SessionTurn.TurnStates CurrentTurnState
    {
        get
        {
            return mCurrentTurn.CurrentState;
        }
        set
        {
            mCurrentTurn.SetState(value);
        }
    }

    private void Start()
    {
        EventManager.Instance.AddHandler<GameStateChangeEvent>(OnGameStateChangedEvent);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveHandler<GameStateChangeEvent>(OnGameStateChangedEvent);
    }

    private void OnGameStateChangedEvent(object sender, GameStateChangeEvent gameStateChangedEvent)
    {
        if (gameStateChangedEvent.NewState == GameManager.GameStates.InGame)
        {
            if (mCurrentState == SessionStates.OutOfSession)
            {
                SetState(SessionStates.Starting);
            }
        }
        else if (gameStateChangedEvent.NewState == GameManager.GameStates.PostGame)
        {
            if (mCurrentState == SessionStates.InProgress)
            {
                SetState(SessionStates.Ending);
            }
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

                DoEndSession();

            break;
        }

        SessionStates previousState = mCurrentState;
        mCurrentState = newState;

        EventManager.Instance.Post(new SessionStateChangedEvent(this, mCurrentState, previousState));

        //HACK
        if (mCurrentState == SessionStates.Starting)
        {
            SetState(SessionStates.InProgress);
        }
        else if (mCurrentState == SessionStates.Ending)
        {
            SetState(SessionStates.OutOfSession);
        }
    }

    private void StartSession()
    {
        mCurrentPlayerIndex = Random.Range(0, PlayerManager.Instance.GetPlayerCount());
        StartCurrentTurn();
    }

    private void DoEndSession()
    {
        //Do whatever is needed to close out the session
        mCurrentTurn.SetState(SessionTurn.TurnStates.NotStarted);
    }

    public void EndSession()
    {
        SetState(SessionStates.Ending);
    }

    public void StartCurrentTurn()
    {
        mCurrentTurn.SetState(SessionTurn.TurnStates.TurnStarting);
    }

    public void EndCurrentTurn()
    {
        mCurrentTurn.SetState(SessionTurn.TurnStates.TurnEnding);
        IterateCurrentPlayer();
        StartCurrentTurn();
    }

    private void IterateCurrentPlayer()
    {
        mCurrentPlayerIndex++;

        if (mCurrentPlayerIndex >= PlayerManager.Instance.GetPlayerCount())
        {
            mCurrentPlayerIndex = 0;
        }
    }
}
