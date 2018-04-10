using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerSessionData
{
    public Player Player;
    public List<CoinScript> CoinsCollected;
}

public class SessionManager : MonoBehaviour
{
    private List<Player> mPlayerList;

    private int mCurrentPlayerIndex = 0;

    public List<Player> PlayerList
    {
        get
        {
            return mPlayerList;
        }
    }
}
