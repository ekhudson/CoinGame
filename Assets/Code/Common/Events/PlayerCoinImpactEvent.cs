using UnityEngine;
using System.Collections;

public class PlayerCoinImpactEvent : EventBase 
{
    public readonly PlayerCoinScript PlayerCoin;
    public Collision CollisionData;

    public PlayerCoinImpactEvent(Object sender, PlayerCoinScript playerCoin, Collision collision) : base(collision.transform.position, sender)
    {       
        PlayerCoin = playerCoin;
        CollisionData = collision;
    }
}
