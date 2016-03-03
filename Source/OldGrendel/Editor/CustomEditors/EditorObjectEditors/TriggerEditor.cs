using System.Collections;

using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(TriggerObject))]
[CanEditMultipleObjects] 
public class TriggerEditor : EditorObjectEditor<TriggerObject>
{	
	protected override void OnEnable()
	{		
		base.OnEnable();	
	}
	
	protected override void OnSceneGUI()
	{
		base.OnSceneGUI();		
		
		_target.GetComponent<SphereCollider>().radius = Handles.RadiusHandle(Quaternion.identity, _target.transform.position, _target.GetComponent<SphereCollider>().radius, true); 
		
	}
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		GUILayout.Label("TRIGGER!");			
	}
	
}
