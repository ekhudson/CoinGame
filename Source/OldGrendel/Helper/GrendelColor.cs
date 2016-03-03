using UnityEngine;
using System.Collections;

public static class GrendelColor
{	
	public static Color DarkGreen = new Color(0, 0.5f, 0, 1);
	public static Color DarkBlue = new Color(0, 0, 0.5f, 1);
	public static Color DarkRed = new Color(0.5f, 0, 0, 1);
	public static Color DarkMagenta = new Color(0.5f, 0, 0.5f, 1);
	public static Color DarkYellow = new Color(0.5f, 0.5f, 0, 1);
	public static Color DarkCyan = new Color(0, 0.5f, 0.5f, 1);
	public static Color DarkOrange = new Color(1, 0.5f, 0, 1);
	public static Color Orange = new Color(0.75f, 0.25f, 0, 1);
	public static Color Pink = new Color(1, 0, 0.5f, 1);
	public static Color DarkPink = new Color(0.5f, 0, 0.25f, 1);
	public static Color GrendelYellow = new Color(1, 0.88f, 0, 1);
    public static Color Teal = new Color(0f, 0.5f, 0.5f, 1f);
    public static Color SeaGreen = new Color(0.18f, 0.54f, 0.34f, 1f);
    public static Color SlateGrey = new Color(0.18f, 0.3f, 0.3f, 1f);
		
	public static Color RandomColor
	{
		get{ return new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f), 1); }
	}
	
	public static Color CustomAlpha(Color color, float alpha)
	{
		alpha = Mathf.Clamp(alpha, 0f, 1f);
		return new Color(color.r, color.g, color.b, alpha);
	}
	
	public static Color FlashingColor(Color color, float time)
	{
		float alpha = Mathf.PingPong( (Time.realtimeSinceStartup * time), 1f);
		//float alpha = Random.Range(0f,1f);
		return new Color(color.r, color.g, color.b, alpha); 
	}
	
}
