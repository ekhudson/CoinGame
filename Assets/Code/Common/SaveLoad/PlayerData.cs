using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerData : CoinSerializableObject 
{
    public string PlayerName = "New Player";
    private int mCurrentScore = 0;
    private bool mInvertYAxis = false;

    protected override string InternalDefaultFileName
    {
        get { return "PlayerData.dat"; }
    }

    protected override string InternalSaveDirectory
    {
        get { return Application.persistentDataPath; }
    }

    public override string DefaultFileName
    {
        get { return InternalDefaultFileName; }
    }

    public override string SaveDirectory
    {
        get { return InternalSaveDirectory; }
    }

    public PlayerData (string name, int score)
    {
        PlayerName = name;
        mCurrentScore = score;
    }

    public int GetCurrentScore()
    {
        return mCurrentScore;
    }

    public void AddToScore(int amount)
    {
        mCurrentScore += amount;
    }
}
