using UnityEngine;

public class SetGameState : MonoBehaviour
{
    [SerializeField]
    private GameManager.GameStates m_GameState;

    public void SetState()
    {
        GameManager.Instance.TrySetState(m_GameState);
    }
}
