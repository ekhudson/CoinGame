using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(EntityManager))]
public class EntityManagerEditor : GrendelEditor<EntityManager>
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		foreach(KeyValuePair<int, Entity> entry in EntityManager.EntityDictionary)
		{
			GUILayout.BeginHorizontal();


			GUILayout.Label(entry.Key.ToString());

			if(GUILayout.Button(entry.Value.name))
			{
				EditorGUIUtility.PingObject(entry.Value.gameObject);
			}	

			GUILayout.EndHorizontal();
		}

	}
}
