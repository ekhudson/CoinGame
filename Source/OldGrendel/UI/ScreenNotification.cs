using UnityEngine;
using System.Collections;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Apr 1, 2012
	/// 
	/// Filename: ScreenNotification.cs
	/// 
	/// Summary: Screen Notifications are text that appear in the topleft
	/// of the screen and fade after a short time. Useful for displaying
	/// text to the user briefly, and letting them know that an action
	/// has taken place (for example, console commands activated while console
	/// is not open)
	/// 
	/// </summary>

public class ScreenNotification
{	
	private float _defaultHoldTime = 0.75f;
	private float _defaultFadeTime = 0.5f;
	private GUIStyle _defaultGUIStyle = new GUIStyle();
	private Texture2D _defaultTexture;
	private Color _defaultBoxColor = new Color(0.25f, 0.25f, 0.25f, 0.75f);
	private string _textToShow = "";
	private Rect _notificationBox = new Rect(10f,10f, (Screen.width), 25f);
	private GUIStyle _styleToUse;
	
	public float HoldTime
	{
		get{return _defaultHoldTime;}
	}
	
	public float FadeTime
	{
		get{return _defaultFadeTime;}
	}
	
	public GUIStyle NotificationStyle
	{
		get{return _styleToUse;}
	}
	
	public string Text
	{
		get{ return _textToShow; }
	}
	
	public Color BoxColor
	{
		get{return _defaultBoxColor;}
		set{_defaultBoxColor = value;}
	}
		
	public ScreenNotification(string text)
	{
		_textToShow = text;
		CreateTexture();
		AddToNotificationList();
	}
	
	public ScreenNotification(string text, Color color)
	{		
		_textToShow = text;
		_styleToUse = new GUIStyle();
		_styleToUse.normal.textColor = color;
		CreateTexture();
		AddToNotificationList();
	}
	
	public void CreateTexture()
	{
		_defaultTexture = new Texture2D(1,1);
		_defaultTexture.SetPixel(1,1, _defaultBoxColor);
		_defaultTexture.Apply();
		
		_styleToUse.normal.background = _defaultTexture;
	}
	
	// Use this for initialization
	void Start () 
	{		
		_defaultGUIStyle.normal.textColor = Color.white;		
	}	
	
	public void DisplayNotification()
	{
		if (_styleToUse == null) {_styleToUse = _defaultGUIStyle; }
		
		_defaultTexture.SetPixel(1,1, _defaultBoxColor);
		_defaultTexture.Apply();
		_styleToUse.normal.background = _defaultTexture;
		_styleToUse.alignment = TextAnchor.MiddleLeft;
		_styleToUse.contentOffset = new Vector2(10,0);
		
		GUI.Box(_notificationBox, _textToShow, _styleToUse	);		
	}
	
	public void AddToNotificationList()
	{
		ScreenNotificationManager.Instance.AddNotification(this);		
	}
	
	public void RemoveFromNotificationList()
	{
		ScreenNotificationManager.Instance.RemoveNotification(this);
	}
	
		
}
