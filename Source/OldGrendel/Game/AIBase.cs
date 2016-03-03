using UnityEngine;
using System.Collections;
using System.Collections.Generic;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Feb 24, 2012
	/// 
	/// Filename: AIBase.cs
	/// 
	/// Summary: Handles fundamental AI Behavior (ie. core logic).
	/// Primarily, other entities are tracked here and sorted
	/// by layer
	/// 
	/// </summary>

[RequireComponent(typeof(Entity))] 
public class AIBase : MonoBehaviour 
{

	protected Entity ParentEntity;
	protected SearchRadius ParentSearchRadius;
	
	protected List<Collider> NearbyColliders = new List<Collider>();
	
	// Use this for initialization
	virtual public void Start () 
	{
		
		ParentEntity = gameObject.GetComponent<Entity>(); //find the entity we are attached to	
		ParentSearchRadius = gameObject.GetComponent<SearchRadius>();
	}
	
	// Update is called once per frame
//	virtual public void Update () 
//	{
//	
//	}
	
	virtual public void CalledUpdate()
	{		
		//put logic here		
	}
	
	virtual public void FindNearbyObjects()
	{			
		NearbyColliders = ParentSearchRadius.ObjectList;		
	}
	
	virtual public void SortNearbyObjects()
	{
		
	}
	
}//end class
