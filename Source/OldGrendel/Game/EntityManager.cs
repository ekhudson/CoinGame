using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityManager : MonoBehaviour {

	public int UpdateAmount = 5;
	public int CreationAmount = 5;
	public static int MaxEntities = 10000; //limit the max number of entities allowed in the scene
	public static Dictionary<int, Entity> EntityDictionary = new Dictionary<int, Entity>();	//list of entities
	
	private static int _lastOpenIndex = 0; //a reference to the last open index for adding new entities
	private static Entity[] _toUpdateArray = new Entity[0];
	private int _updateIndex = 0; //current spot in the _toUpdate Array
	private int _updateTargetIndex = 0; //where we'll end our update cycle
	//private static List<int> _removeList = new List<int>(); //for removing entities from the EntityDictionary during updating
	//private static List<Entity> _addList = new List<Entity>(); //for adding entities to the EntityDictionary during updating
	private static Entity _testEntity; //this test entity is used for updating
	
	// Use this for initialization
	void Awake () 
	{		
		_toUpdateArray = new Entity[MaxEntities];
	}
	
	void Start()
	{
		StartCoroutine( ManagedUpdate() );
	}	
	
	// Update is called once per frame
//	void Update () {
//		
////		_updateTargetIndex += UpdateAmount;
////		
////		for ( ; _updateIndex <= _updateTargetIndex && _updateIndex < MaxZombies && Time.deltaTime < 0.08f; _updateIndex++)
////		{
////			//Debug.Log("Index: " + _updateIndex);
////			_tempZombie = _toUpdateArray[_updateIndex];
////			if(_tempZombie != null){ _toUpdateArray[_updateIndex].CalledUpdate(); }
////						
////		}
////		
////		if ( _updateIndex >= (MaxZombies - 1) ) {_updateTargetIndex = 0; _updateIndex = 0;} //reset the cycle
//		
//			
//	}
	
	IEnumerator ManagedUpdate()
	{		
		while (true)
		{				
			_updateTargetIndex += UpdateAmount;			
		
			for ( ; _updateIndex <= _updateTargetIndex && _updateIndex < MaxEntities; _updateIndex++)
			{				
				_testEntity = _toUpdateArray[_updateIndex];				
				if(_testEntity != null)
				{ 
					_testEntity.CalledUpdate(); 
				}				
			}			
			
			if ( _updateIndex >= (MaxEntities - 1) ) {_updateTargetIndex = 0; _updateIndex = 0;} //reset the cycle
			
			yield return new WaitForSeconds(Time.deltaTime); //pace the coroutine
		}
	}	
			
//			//initialize our remove array, and simultaneously remove any entities in that array		
//			foreach(int key in _removeList)
//			{				
//				if (key == null){ continue; }
//				else
//				{
//					EntityDictionary.Remove(key);
//				}				
//			}
//			_removeList.Clear();			
//			
//			//initialize our add array, and simultaneously add any entities in that array
//			foreach(Entity entity in _addList)
//			{				
//				if (entity == null){ continue; }
//				else
//				{
//					EntityDictionary.Add(entity.BaseInstanceID, entity);
//				}				
//			}
//			_addList.Clear();
//			
//			foreach(int key in EntityDictionary.Keys)
//			{					
//				_testEntity = EntityDictionary[key]; //grab a referene to the entity to be updated
//				
//				//look for dead entities and remove them
//				if (_testEntity.ToBeKilled)
//				{
//					_removeList.Add(key);					
//					_testEntity.KillEntity(); 
//					continue;
//				}
//				
//				_testEntity.CalledUpdate(); //update valid entities								
//				_updateIndex++; //increment
//				
//				if (_updateIndex >= _updateTargetIndex)
//				{
//					yield return new WaitForSeconds(Time.deltaTime);	
//				}
//				
//				_updateTargetIndex += UpdateAmount;
//			}			
//			
//			if ( _updateIndex >= (MaxZombies - 1) ) {_updateTargetIndex = 0; _updateIndex = 0;} //reset the cycle		
					
	public static void AddToUpdateList(Entity entity)
	{		
		if (_lastOpenIndex < MaxEntities)
		{			
			_toUpdateArray[_lastOpenIndex] = entity;
			_lastOpenIndex++;
		}
		else
		{
			Destroy(entity.BaseGameObject);
			Debug.LogWarning("Too many entities!");
		}					
	}	
	
	/// <summary>
	/// AddToDictionary
	/// 
	/// This function is called by entities when they wake, adding them
	/// to the list for managed updates and for quick finding through InstanceID
	/// 
	/// </summary>
	/// <param name="ID">
	/// A <see cref="System.String"/>
	/// </param>
	/// <param name="entity">
	/// A <see cref="Entity"/>
	/// </param>
	public static void AddToDictionary(int ID, Entity entity)
	{		
		EntityDictionary[ID] = entity; //add the entity to the dictionary		
	}
	
	/// <summary>
	/// Removes from dictionary.
	/// </summary>
	/// <param name='ID'>
	/// I.
	/// </param>
	public static void RemoveFromDictionary(int ID)
	{
		try
		{
			EntityDictionary.Remove(ID); //attempt to remove an entity from the dictionary
		}
		catch
		{
			Debug.LogWarning("Entity Dictionary does not contain an entry for Entity " + ID);
		}
	}

}
