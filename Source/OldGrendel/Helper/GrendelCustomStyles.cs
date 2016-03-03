using UnityEngine;
using System.Collections;

public static class GrendelCustomStyles
{	
	
	public static GUIStyle CustomElement(GUIStyle style, Color color, Color textColor)
	{	
		return CustomElement(style, color, textColor, style.alignment, style.fontStyle, style.fontSize);
	}
	
	public static GUIStyle CustomElement(GUIStyle style, Color color, Color textColor, TextAnchor textAlignment)
	{	
		return CustomElement(style, color, textColor, textAlignment, style.fontStyle, style.fontSize);
	}
	
	public static GUIStyle CustomElement(GUIStyle style, Color color, Color textColor, TextAnchor textAlignment, FontStyle fontStyle)
	{	
		return CustomElement(style, color, textColor, style.alignment, fontStyle, style.fontSize);
	}	
	
	public static GUIStyle CustomElement(GUIStyle style, Color color, Color textColor, TextAnchor textAlignment, FontStyle fontStyle, int fontSize)
	{
		GUIStyle newStyle = new GUIStyle(style);
		newStyle.fontStyle = fontStyle;
		newStyle.fontSize = fontSize;
		newStyle.alignment = textAlignment;
		newStyle.normal.textColor = textColor;
		
		return newStyle;		
	}
	
	
	
}
