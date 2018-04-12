using UnityEngine;

public class DebugAddPlayers : MonoBehaviour
{
    [SerializeField]
    private PlayerData[] m_PlayersToAdd;

    private void Start()
    {
        foreach(PlayerData player in m_PlayersToAdd)
        {
            PlayerManager.Instance.AddPlayer(player);
        }
    }
}
