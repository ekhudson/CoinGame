using UnityEngine;
using System.Collections;


	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Mar 2, 2012
	/// 
	/// Filename: Factions.cs
	/// 
	/// Summary: Handles the different Factions and
	/// their relationship to one another. This information
	/// is used by AI for various purposes, and could
	/// potentially be used for other purposes
	/// (such as filtering damage from allies)
	/// 
	/// </summary>


public class FactionManager : Singleton<FactionManager>
{
	public enum Factions { Human, Zombie, Sasquatch, Vampire, Other };
	
	[System.Serializable]
	public class FactionData
	{
		public Factions Faction;
		public Factions[] Allies;
		public Factions[] Enemies;
		public Factions[] Neutral;
	}
	
	public FactionData[] FactionList;
		
	public string[] FactionNames
	{	
		get { return System.Enum.GetNames(typeof(Factions)); }
	}
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
