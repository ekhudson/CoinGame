using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(EditorObjectManager))]
public class EditorObjectManagerEditor : Editor
{
	
	virtual public EditorObjectManager Target
	{		
		get {return target as EditorObjectManager;}		
	}
	
	public override void OnInspectorGUI()
	{
		SerializedObject obj = new SerializedObject(Target);
		obj.Update();
		
		
		foreach(KeyValuePair<EditorObject, List<EditorObjectConnection>> pair in Target.ConnectionRegistry)
		{
			foreach(EditorObjectConnection connection in pair.Value)
			{
				GUI.color = connection.MessageColor;
				GUILayout.Label(string.Format("{0} to {1}", pair.Key.ToString(), connection.Subject.ToString()), GUI.skin.box);
				GUI.color = Color.white;
			}
		}		
		obj.ApplyModifiedProperties();
				
	}
}

