using UnityEngine;
using System.Collections;

public static class StringTool
{
	public static string Truncate(string source, int length)
    {
		if (source.Length > length)
		{
	    	source = source.Substring(0, length);
		}
		
		return source;
    }
	
	public static string ForceLength(string source, int length)
	{
		if (source.Length > length)
		{
			source = Truncate(source, length);				
		}
		else
		{
			while (source.Length < length)
			{
				source += " ";
			}
		}
		
		return source;
		
	}
}
