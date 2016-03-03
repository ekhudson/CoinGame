using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIManager))]
public class UIManagerEditor : GrendelEditor<UIManager>
{	

	public override void OnInspectorGUI()
	{
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();

        Target.OverrideGamestate = EditorGUILayout.BeginToggleGroup("Override GameState", Target.OverrideGamestate);
           Target.OverrideState = (GameManager.GameStates.STATES)EditorGUILayout.EnumPopup(Target.OverrideState);
        EditorGUILayout.EndToggleGroup();

        EditorGUILayout.EndHorizontal();

	}

}

