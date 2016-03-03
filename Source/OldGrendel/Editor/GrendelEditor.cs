using System.Reflection;
using System.Collections;

using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
public class GrendelEditor<T> : Editor where T : class
{
    public T Target
	{
		get{return target as T;}		
	}
	
	public override void OnInspectorGUI()
	{
        GUILayout.Label(string.Format("Base type: {0}", Target.GetType().BaseType.ToString()));
        base.OnInspectorGUI();
	}


}
