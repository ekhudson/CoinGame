using UnityEngine;
using System.Collections;

public class ScoreboardCoin : MonoBehaviour 
{
    public GameObject ScoreboardCoinPrefab;

    private GameObject mScoreboardCoin = null;
    private Rigidbody mScoreboardCoinRigidbody;

    private static Camera sCoinCam;

    public static Camera CoinCam
    {
        get
        {
            return sCoinCam;
        }
    }

    private void Start()
    {
    }

    private void LateUpdate()
    {

    }
}
