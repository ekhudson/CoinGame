using Sirenix.OdinInspector;
using UnityEngine;

public class SessionManager : Singleton<SessionManager>
{
    public enum SessionStates
    {
        InProgress,
        OutOfSession,
    }

    private int mCurrentPlayerIndex = 0;

    [SerializeField]
    [ReadOnly]
    private SessionStates mCurrentState = SessionStates.OutOfSession;
    [SerializeField]
    [ReadOnly]
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
            SetState(SessionStates.InProgress);
        }
        else if (gameStateChangedEvent.NewState == GameManager.GameStates.PostGame)
        {
            SetState(SessionStates.OutOfSession);
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

            case SessionStates.InProgress:

            break;                
        }

        SessionStates previousState = mCurrentState;
        mCurrentState = newState;

        EventManager.Instance.Post(new SessionStateChangedEvent(this, mCurrentState, previousState));
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

    }

    public void StartCurrentTurn()
    {
        mCurrentTurn.SetState(SessionTurn.TurnStates.Aiming);
    }

    public void EndCurrentTurn()
    {
        mCurrentTurn.SetState(SessionTurn.TurnStates.TurnEnded);
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
