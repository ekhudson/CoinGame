using UnityEngine;
using UnityEngine.Events;

public class SessionStateChangedEventHandler : MonoBehaviour
{
    [SerializeField]
    private SessionManager.SessionStates m_GameState;

    [SerializeField]
    private UnityEvent m_OnStateChanged;

    private void Start()
    {
        EventManager.Instance.AddHandler<SessionStateChangedEvent>(SessionStateChangedHandler);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveHandler<SessionStateChangedEvent>(SessionStateChangedHandler);
    }

    private void SessionStateChangedHandler(object sender, SessionStateChangedEvent sessionStateChangedEvent)
    {
        if (sessionStateChangedEvent.NewState == m_GameState)
        {
            if (m_OnStateChanged != null)
            {
                m_OnStateChanged.Invoke();
            }
        }
    }
}
