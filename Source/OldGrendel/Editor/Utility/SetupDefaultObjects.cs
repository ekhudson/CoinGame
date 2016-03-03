using UnityEngine;
using UnityEditor;

using System.Collections;

/// <summary>
/// Title: Grendel Framework
/// Author: Elliot Hudson
/// Date: Mar 28, 2012
/// 
/// Filename: SetupDefaultObjects.cs
/// 
/// Summary: Checks the scene for the existence of the
/// default objects required by the Grendel Framework,
/// and adds any that are missing.
/// 
/// </summary>
/// 	

//TODO: Add some checking for missing prefabs below


public class SetupDefaultObjects : Editor
{
	private static Object[] RequiredObjects = new Object[] { Resources.Load("Prefabs/Grendel/$GameManager"),
															 Resources.Load("Prefabs/Grendel/$LevelManager"),
															 Resources.Load("Prefabs/Grendel/$ConnectionRegistry")
														   };
		
	
	[MenuItem ("Grendel/Setup Default Objects")]	
	private static void SearchForObjects()
	{
		string results = string.Empty;
		
		foreach(Object obj in RequiredObjects)
		{
			Debug.Log(string.Format("Searching for {0} in the scene.", obj.name));
			if (GameObject.Find(obj.name) == null)
			{
				Debug.Log(string.Format("{0} not found, adding it now.", obj.name));
				PrefabUtility.InstantiatePrefab(obj);
				results += string.Format("{0} added\n", obj.name);
			}
			else
			{
				Debug.Log(string.Format("{0} already exists.", obj.name));
			}
			
		}		
		
		//Display results if there are any
		if (!string.IsNullOrEmpty(results))
		{
			EditorUtility.DisplayDialog("Setup Default Grendel Objects", results, "OK");
		}
	}
}

