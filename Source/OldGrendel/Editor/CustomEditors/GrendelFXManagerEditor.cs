using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(GrendelFXManager))]
public class GrendelFXManagerEditor : GrendelEditor<GrendelFXManager> 
{
	private const float kBrowseButtonWidth = 32f;

	private void OnEnable()
	{
		RefreshFXLibrary();
	}

	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();
		DrawDefaultInspector();
		DrawLibraryPathButton();

		if (EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(Target);
		}
	}

	private void DrawLibraryPathButton()
	{
		string buttonText = "...";
		Color buttonColor = Color.white;

		GUILayout.BeginHorizontal();

		EditorGUILayout.PrefixLabel("FX Library Path");

		if(string.IsNullOrEmpty(Target.FXLibraryPath))
		{
			buttonText = "Library Path not set! Click to browse";
			buttonColor = Color.yellow;
		}
		else
		{
			EditorGUILayout.SelectableLabel(Target.FXLibraryPath);
		}

		GUI.color = buttonColor;

		if (GUILayout.Button(buttonText, GUILayout.Width(kBrowseButtonWidth)))
		{
			Target.FXLibraryPath = EditorUtility.OpenFolderPanel("Choose FX Library Path", Target.FXLibraryPath, Target.FXLibraryPath);
		}

		GUI.color = Color.white;

		GUILayout.EndHorizontal();
	}

	private void RefreshFXLibrary()
	{

	}
}
