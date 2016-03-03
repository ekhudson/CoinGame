using UnityEngine;
using System.Collections;

public class CoinEvent : EventBase 
{
    public readonly CoinScript Coin;
    public readonly CoinEventTypes CoinEventType;

    public enum CoinEventTypes
    {
        LANDED_FACE_UP,
    }
    
    public CoinEvent(Object sender, CoinScript coin, CoinEventTypes coinEventType) : base(coin.transform.position, sender)
    {       
        Coin = coin;
        CoinEventType = coinEventType;
    }
}
