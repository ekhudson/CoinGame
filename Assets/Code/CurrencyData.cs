using System.Collections;
using UnityEngine;

[System.Serializable]
public class CurrencyData
{
    [SerializeField]    private string mName = string.Empty;
    [SerializeField]    private string mColloquialName = string.Empty;
    [SerializeField]    private float mExchangeRateToStandard = 1f;
    [SerializeField]    private CoinSet[] mCoinSets;

                        public string Name { get { return mName; } }
                        public string ColloquialName { get { return mColloquialName; } }
                        public float ExchangeRateToStandard { get { return mExchangeRateToStandard; } }
                        public CoinSet[] CoinSets { get { return mCoinSets; } }
}


