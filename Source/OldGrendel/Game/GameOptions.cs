using UnityEngine;
using System.Collections;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Feb 20, 2012
	/// 
	/// Filename: GameOptions.cs
	/// 
	/// Summary: Handles game options at launch and when changed at runtime.
	/// Resides on the GameManager and is carried from scene to scene
	///  
	/// </summary>

public class GameOptions : MonoBehaviour 
{	
	public bool DebugMode = false;
	
	public bool HideDefaultCursor = false; //hide the default mouse cursor	
	
	private bool _isDirty = false; //have options changed?
	
	private static GameOptions _instance;
	
	static public GameOptions Instance
	{
		get{return _instance;}
	}
	
	void Awake()
	{
		_instance = this;
	}
	
	// Use this for initialization
	void Start () 
	{
		CheckOptions();	
	}
	
	// Update is called once per frame
	void Update () 
	{	
		if (_isDirty){ CheckOptions(); }		
	}
	
	
	void HideMouseCursor(bool hide)
	{
		if (hide) {Cursor.visible = false;}
        else {Cursor.visible = true; }
	}
	
	void CheckOptions()
	{
		if (HideDefaultCursor){HideMouseCursor(true);} //HideDefaultCursor
	}
	
	//ACCESSORS	
	
	public bool IsDirty
	{
		get {return _isDirty;}
		set {_isDirty = value;}
	}
}
