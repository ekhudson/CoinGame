using System.Collections;

using UnityEngine;
using UnityEditor;

public class GrendelPrefencesWindow 
{
	[PreferenceItem ("Grendel")]
	public static void PreferencesGUI()
	{
		EditorGUI.BeginChangeCheck();
		
		GrendelEditorPreferences.DrawEditorObjectLabels = GUILayout.Toggle(GrendelEditorPreferences.DrawEditorObjectLabels, "Draw Editor Object Labels");
		
		if (EditorGUI.EndChangeCheck())
		{
			SceneView.currentDrawingSceneView.Repaint(); 
		}

        GrendelEditorPreferences.AskToLoadStartupScene = GUILayout.Toggle(GrendelEditorPreferences.AskToLoadStartupScene, new GUIContent("Ask To Load Default Scene", "When playing in the Editor, always ask to load the default scene if it's not the current scene"));
	}
}
