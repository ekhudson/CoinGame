using System;
using System.Collections;

using UnityEngine;

[System.Serializable]
public class EditorObjectConnection : ScriptableObject
{	
	//PUBLIC VARIABLES
	[SerializeField]public EditorObject Caller;
	[SerializeField]public EditorObject Subject;	
	[SerializeField]public EditorObject.EditorObjectMessage Message = EditorObject.EditorObjectMessage.Activate;	
	[SerializeField]public EventTransceiver.Events OnEvent;
	
	//PUBLIC VARIABLES (HIDDEN)
	[HideInInspector]
	public string GUID; //used to easily match connections when testing between objects
	
	//PROTECTED VARIABLES
	protected Color _messageColor;
	protected Color _messageColorDark;
	
	//PROPERTIES
	public Color MessageColor
	{
		get 
		{ 			
			SetColor();			
			return _messageColor; 			
		}
	}
	
	public Color MessageColorDark
	{
		get { return _messageColorDark; }
	}	
	
	//Constructor
	public EditorObjectConnection(EditorObject.EditorObjectMessage message)
	{
		Message = message;
		GUID = System.Guid.NewGuid().ToString();
		SetColor();		
	}
	
	//Constructor
	public EditorObjectConnection(EditorObject.EditorObjectMessage message, EditorObject caller, EditorObject subject, EventTransceiver.Events onEvent)
	{
		OnEvent = onEvent;
		Message = message;
		Caller = caller;
		Subject = subject;
		GUID = System.Guid.NewGuid().ToString();
		SetColor();		
	}
	
	public void SetColor()
	{		
		switch(Message)
		{
				case EditorObject.EditorObjectMessage.None:						
					_messageColor =	Color.white;
					_messageColorDark = Color.gray;					
				break;
		
				case EditorObject.EditorObjectMessage.Activate:					
					_messageColor =	Color.green;
					_messageColorDark = GrendelColor.DarkGreen;
				break;
			
				case EditorObject.EditorObjectMessage.Deactivate:	
					_messageColor =	Color.red;
					_messageColorDark = GrendelColor.DarkRed;
				break;
			
				case EditorObject.EditorObjectMessage.Toggle:	
					_messageColor =	Color.yellow;
					_messageColorDark = GrendelColor.DarkYellow;
				break;
			
				case EditorObject.EditorObjectMessage.Enable:	
					_messageColor = Color.magenta;
					_messageColorDark = GrendelColor.DarkMagenta;
				break;
			
				case EditorObject.EditorObjectMessage.Disable:
					_messageColor = GrendelColor.Orange;
					_messageColorDark =	GrendelColor.DarkOrange; 
				break;			
				
				default:					
					_messageColor =	Color.grey; //TODO: Change to dark colours					
				break;
		}		
	}
}

