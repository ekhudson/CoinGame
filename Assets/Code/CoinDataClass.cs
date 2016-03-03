using System.Collections;
using UnityEngine;

public class CoinDataClass 
{
    public enum CoinClassTypes
    {
        Coin,
        Token,
        Award,
        Badge,
    }

    public enum CoinMaterialTypes
    {
        Wood,
        Copper,
        Nickle,
        Bronze,
        Silver,
        Gold,
        Platinum,
    }

    [System.Serializable]
    public class CoinData
    {
        public string Name = string.Empty;
        public string ColluquialName = string.Empty;
        public CoinDataClass.CoinClassTypes CoinType = CoinClassTypes.Coin;
        public CoinDataClass.CoinMaterialTypes CoinMaterial = CoinMaterialTypes.Nickle;
        public float Value = 1f;
        public GameObject Model;
    }
}


