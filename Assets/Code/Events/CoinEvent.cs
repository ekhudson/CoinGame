using UnityEngine;
using System.Collections;

public class CoinEvent : EventBase 
{
    public readonly CoinScript Coin;
    public readonly CoinEventTypes CoinEventType;

    public enum CoinEventTypes
    {
        IMPACTED,
        SETTLED_FACE_UP,
        SETTLED_FACE_DOWN,
    }
    
    public CoinEvent(Object sender, CoinScript coin, CoinEventTypes coinEventType) : base(coin.transform.position, sender)
    {       
        Coin = coin;
        CoinEventType = coinEventType;
    }
}
