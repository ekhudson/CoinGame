using UnityEngine;
using UnityEngine.Events;

public class TurnStateChangedEventHandler : MonoBehaviour
{
    [SerializeField]
    private SessionTurn.TurnStates m_GameState;

    [SerializeField]
    private UnityEvent m_OnStateChanged;

    private void Start()
    {
        EventManager.Instance.AddHandler<TurnStateChangedEvent>(SessionStateChangedHandler);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveHandler<TurnStateChangedEvent>(SessionStateChangedHandler);
    }

    private void SessionStateChangedHandler(object sender, TurnStateChangedEvent turnStateChangedEvent)
    {
        if (turnStateChangedEvent.NewState == m_GameState)
        {
            if (m_OnStateChanged != null)
            {
                m_OnStateChanged.Invoke();
            }
        }
    }
}
