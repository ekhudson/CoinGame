using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(GrendelAudioEntry))]
public class GrendelAudioEntryPropertyDrawer : PropertyDrawer 
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) 
	{
		if (Application.isPlaying)
		{
			GUILayout.Label("Audio Clip changes not available while playing!");
			return;
		}

		if (GrendelManager.ProjectData == null)
		{
			GUILayout.Label("Project Data Missing");
			return;
		}

		string[] bankNames = new string[GrendelManager.ProjectData.AudioOptions.AudioBanks.Count];

		int count = 0;

		foreach(GrendelAudioBank bank in GrendelManager.ProjectData.AudioOptions.AudioBanks)
		{
			bankNames[count] = bank.BankName;
			count++;
		}

		SerializedProperty bankNumber = null;
		SerializedProperty clipNumber = null;
		label = EditorGUI.BeginProperty(position, label, property);
		property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true);
		EditorGUI.EndProperty();

		EditorGUI.indentLevel++;

		EditorGUILayout.BeginVertical();

		bankNumber = property.FindPropertyRelative("AudioBankNumber");
		clipNumber = property.FindPropertyRelative("AudioClipNumber");

		bankNumber.intValue = EditorGUILayout.Popup("Audio Bank", bankNumber.intValue, bankNames);

		string[] clipNames = new string[GrendelManager.ProjectData.AudioOptions.AudioBanks[bankNumber.intValue].AudioClips.Count];

		count = 0;

		foreach(AdjustableAudioClip clip in GrendelManager.ProjectData.AudioOptions.AudioBanks[bankNumber.intValue].AudioClips)
		{
			clipNames[count] = clip.Clip.name;
			count++;
		}

		clipNumber.intValue = EditorGUILayout.Popup("Audio Clip", clipNumber.intValue, clipNames);

		GUILayout.Label("Preview:");

		AdjustableAudioClipEditor.StaticOnInspectorGUI(GrendelManager.ProjectData.AudioOptions.AudioBanks[bankNumber.intValue].AudioClips[clipNumber.intValue]);

		EditorGUILayout.EndVertical();

		GUILayout.Label(GrendelManager.ProjectData.AudioOptions.AudioBanks[bankNumber.intValue].BankName);
		GUILayout.Label(GrendelManager.ProjectData.AudioOptions.AudioBanks[bankNumber.intValue].AudioClips[clipNumber.intValue].Clip.name);

		EditorGUI.indentLevel--;

		bankNumber.serializedObject.ApplyModifiedProperties();
		clipNumber.serializedObject.ApplyModifiedProperties();
		property.serializedObject.ApplyModifiedProperties();

	}
}
