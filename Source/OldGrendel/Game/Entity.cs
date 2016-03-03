using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    /// <summary>
    /// Title: Grendel Engine
    /// Author: Elliot Hudson
    /// Date: Jan 15, 2012
    /// 
    /// Filename: Entity.cs
    /// 
    /// Summary: Extends Baseobject. Provides information that is typically
    /// shared across a variety of ingame entities, such as health, movement,
    /// and life/death events. Basically, anything that may influence final
    /// entity logic, but is not specific logic itself... if that makes sense
    /// 
    /// </summary>
    
public class Entity : BaseObject
{
    //PUBLIC VARIABLES
    public bool Managed = true; //is this Entity managed? Entities cannot update themselves, so checking this off requires a custom way to update the entity thereafter
    public bool DestroyedOnDeath = true; //is this Entity destroyed when it dies?
    public FactionManager.Factions Faction; //this Entity's Faction
    public bool Invincible = false; //set to true to prevent this Entity ever taking damage
    public int Health = 100; //the Entity's starting health / max health
    
    //PROTECTED VARIABLES
    protected int mCurrentHealth; //the entity's current health
    protected bool mToBeKilled = false; //this is marked true when the entity is ready to be cleared out by the Manager    
    protected SearchRadius mSearchRadius; //this entities SearchRadius
    protected List<Entity> mNearbyAllies = new List<Entity>(); //allies within the SearchRadius
    protected List<Entity> mNearbyEnemies = new List<Entity>(); //enemies within the SearchRadius;
    
    //PRIVATE VARIABLES
    private Entity _testEntity; //temp Entity used for sorting SearchRadius list
    
    
    /// <summary>
    /// Awake this instance.
    /// </summary>
    protected override void Awake ()
    {        
        base.Awake();
    }

    /// <summary>
    /// Start this instance.
    /// </summary>
    protected override void Start () 
    {            
        base.Start();
        
        EntityManager.AddToDictionary(mInstanceID, this);

        if (Managed)
        {
            EntityManager.AddToUpdateList(this);
        }
        
        mCurrentHealth = Health;
        mSearchRadius = GetComponentInChildren<SearchRadius>();
        
        OnSpawn(); 
    }    
    
//    virtual public int TakeDamage(int amount)
//    {        
//        if (Invincible)
//        {
//            return 0;
//        }
//
//        int dmg = ModifyHealth(-amount);
//        
//        if (mCurrentHealth <= 0)
//        {            
//            KillEntity();
//        }
//        
//        return dmg;
//    }    

    virtual public void KillEntity()
    {
        OnDeath();
                
        if (Managed){EntityManager.RemoveFromDictionary(mInstanceID); } //remove this Entity from the Manager
        if (DestroyedOnDeath) { Destroy(mGameObject); } //destroy the gameObject, if specified
    }
    
    virtual public void CalledUpdate()
    {
        //this update function is called by the Manager
        if (mSearchRadius)
        {
            SortSearchRadiusList();
        }
    }
        
    /// <summary>
    /// ModifyHealth
    /// 
    /// Accepts a single int to modify current health
    /// and returns the result. NOTE: modification is by
    /// addition, so lowering health requires a negative
    /// number
    /// </summary>
    /// <param name="amount">
    /// A <see cref="System.Int32"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Int32"/>
    /// </returns>        
    virtual public int ModifyHealth(object sender, Entity subject, int updateValue)
    {
        if (updateValue < 0 && Invincible)
        {
            return mCurrentHealth;
        }

        mCurrentHealth = Mathf.Clamp(mCurrentHealth + updateValue, 0, Health);

        EventManager.Instance.Post(new HealthUpdateEvent(sender, subject, updateValue, mCurrentHealth, mTransform.position));

        return mCurrentHealth;
    }
    
    private void SortSearchRadiusList()
    {
        foreach(Collider collider in mSearchRadius.ObjectList)
        {
            _testEntity = EntityManager.EntityDictionary[ collider.gameObject.GetInstanceID() ];
            if(_testEntity.Faction == this.Faction)
            {
                mNearbyAllies.Add(_testEntity);
            }
            else
            {
                mNearbyEnemies.Add(_testEntity);
            }
        }
    }	
    
    /// <summary>
    /// Raises the death event.
    /// </summary>
    virtual public void OnDeath()
    {
        
    }
    
    /// <summary>
    /// Raises the spawn event.
    /// </summary>
    virtual public void OnSpawn()
    {
        
    }
    
    /// <summary>
    /// Raises the damage event.
    /// </summary>
    /// <param name='amount'>
    /// Amount.
    /// </param>
    virtual public void OnDamage(int amount)
    {
        
    }    
    
    //ACCESSORS
    public bool ToBeKilled
    {
        get{return mToBeKilled;}
        set{mToBeKilled = value;}
    }
}
