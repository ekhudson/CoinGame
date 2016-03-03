using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ObjectComparer : IComparer<EditorObject>
{
	// Calls CaseInsensitiveComparer.Compare with the parameters reversed.
	public int Compare( EditorObject x, EditorObject y )  
	{
		 return x.name.CompareTo(y.name); 
	}
		
}
