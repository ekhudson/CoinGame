using UnityEngine;
using System.Collections;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Mar 12, 2012
	/// 
	/// Filename: ConsoleCommands.cs
	/// 
	/// Summary: Draws an FPS Counter to the
	/// GUI, useful for testing builds outside
	/// the editor. Includes the frame time in
	/// milliseconds as well
	/// 
	/// </summary>

public class FPSCounter : Singleton<FPSCounter> 
{
	#region PUBLIC VARIABLES

	#endregion
	
	#region PRIVATE VARIABLES
	private bool _bShowFPS = false;	
	private float accum   = 0; // FPS accumulated over the interval
	private int   frames  = 0; // Frames drawn over the interval
	//private float timeleft; // Left time for current interval
	private string textToDisplay;
	private Color textColor = Color.green;
	private GUIStyle _fPSStyle = new GUIStyle();
	
	#endregion	
	
	public bool ShowFPS
	{
		get { return _bShowFPS; }
		set { _bShowFPS = value; }
	}
	
	void OnGUI()
	{		
		if (_bShowFPS){	DrawFPSCounter(); }		
	}
	
	void Update()
	{
		if (_bShowFPS)
		{
			CalculateFPS();
		}
	}
	
	void DrawFPSCounter()
	{		
		_fPSStyle.normal.textColor = textColor;	
		
		GUI.Box(new Rect(10,10,200, 30), "");
		GUI.Box(new Rect(20,20,200, 30), textToDisplay, _fPSStyle);		
	}
	
	void CalculateFPS()
	{		
		accum += Time.timeScale/Time.deltaTime;
		++frames;
		float fps = accum/frames;
		textToDisplay = System.String.Format("FPS: {0:F2} MS: {1} ", fps, (Time.deltaTime * 1000));
		
			
		if(fps < 30)
    		textColor = Color.yellow;			
		else 
    	if(fps < 10)
        	textColor = Color.red;
   		else
        	textColor = Color.green;			
		
    	accum = 0.0F;
    	frames = 0;		
	}
	
}
