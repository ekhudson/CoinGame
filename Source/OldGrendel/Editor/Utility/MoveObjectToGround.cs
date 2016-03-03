using System.Collections;

using UnityEngine;
using UnityEditor;

public class MoveObjectToGround : ScriptableObject
{
	public MoveObjectToGround(GameObject objectToMove)
	{		
		Move(objectToMove);
	}
	
	public static void Move(GameObject objectToMove)
	{
		if (objectToMove == null)
		{
			Debug.LogWarning(string.Format("Tried to move object {0} to the ground, but the object was null.", objectToMove));
		}
		
		Ray ray = new Ray(objectToMove.transform.position, Vector3.down);
		RaycastHit hit = new RaycastHit();
		
		if (Physics.Raycast(ray, out hit, Mathf.Infinity))
		{			
			objectToMove.transform.position = hit.point;
		}
		else
		{
			Debug.LogWarning(string.Format("Tried to move object {0} to the ground, but no ground was found!", objectToMove));
		}
	}
}

