using System.Collections;

using UnityEditor;
using UnityEngine;

using GrendelEditor.UI;

[CustomEditor(typeof(GrendelSpawner))]
[CanEditMultipleObjects] 
public class GrendelSpawnerEditor : EditorObjectEditor<GrendelSpawner>
{	
	
	protected override void OnEnable()
	{		
		base.OnEnable();	
	}
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();								
	}
	
	protected override void OnSceneGUI()
	{
		base.OnSceneGUI();		
		
	}
	
}

