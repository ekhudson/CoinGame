using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(GrendelAudioData))]
public class GrendelAudioDataEditor : GrendelEditor<GrendelAudioData>
{
    private static bool mToggleAudioOptions = false;
    private const float kDeleteButtonWidth = 48f;

	public static void StaticOnInspectorGUI(GrendelAudioData target)
    {
        mToggleAudioOptions = GUILayout.Toggle(mToggleAudioOptions, "Audio Options", EditorStyles.toolbarButton);

        if (!mToggleAudioOptions)
        {
           return;
        }

        EditorGUI.indentLevel++;

        GUILayout.BeginVertical();

            GUILayout.Label("Audio Channels: ");

            EditorGUI.indentLevel++;

            for (int i = 0; i < target.AudioChannels.Count; i++)
            {
				GUILayout.BeginVertical(EditorStyles.textField);

					EditorGUILayout.Space();

					EditorGUILayout.SelectableLabel(target.AudioChannels[i].ChannelName, EditorStyles.whiteLabel);

					GUILayout.BeginHorizontal();

	                target.AudioChannels[i].ChannelName = EditorGUILayout.TextField("Channel Name", target.AudioChannels[i].ChannelName);

	                if (GUILayout.Button(new GUIContent("X", "Delete Channel"), GUILayout.Width(kDeleteButtonWidth)))
	                {
	                    if (!EditorUtility.DisplayDialog("Delete Audio Channel", string.Format("Are you sure you want to delete the {0} Audio Channel?", target.AudioChannels[i].ChannelName), "Delete", "Cancel"))
	                    {
	                        break;
	                    }

	                    target.AudioChannels.RemoveAt(i);
	                    GUILayout.EndHorizontal();
	                    break;
	                }

	                GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal();

					target.AudioChannels[i].ChannelVolume = (float)System.Math.Round(EditorGUILayout.Slider("Channel Volume", target.AudioChannels[i].ChannelVolume, 0, 1), 2);

					GUILayout.EndHorizontal();

					EditorGUILayout.Space();

				GUILayout.EndVertical();
            }

            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Add New Channel", GUILayout.ExpandWidth(false)))
            {
                target.AudioChannels.Add(new GrendelAudioChannel());
            }

            GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        EditorGUI.indentLevel--;

        EditorGUILayout.Space();

        GUILayout.Label("Audio Banks: ");

        EditorGUI.indentLevel++;

        for (int j = 0; j < target.AudioBanks.Count; j++)
        {
            EditorGUILayout.Space();

            EditorGUI.indentLevel--;

            GUILayout.BeginVertical(EditorStyles.textField);

            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();

            target.AudioBanks[j].BankName = EditorGUILayout.TextField("Bank Name", target.AudioBanks[j].BankName);

            if (GUILayout.Button(new GUIContent("X", "Delete Audio Bank"), GUILayout.Width(kDeleteButtonWidth)))
            {
                target.AudioBanks.RemoveAt(j);
                GUILayout.EndHorizontal();
                break;
            }

            GUILayout.EndHorizontal();

            EditorGUI.indentLevel++;

            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Bank Assets Location", string.Empty, EditorStyles.wordWrappedMiniLabel);

            if (GUILayout.Button(target.AudioBanks[j].LocationOfBankAssets))
            {
                Selection.activeObject = AssetDatabase.LoadAssetAtPath( FileUtil.GetProjectRelativePath(target.AudioBanks[j].LocationOfBankAssets), typeof( Object ));
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(new GUIContent("...", "Select Bank Assets Location"), GUILayout.Width(kDeleteButtonWidth)))
            {
                if (!string.IsNullOrEmpty(target.AudioBanks[j].LocationOfBankAssets))
                {
                    if(!EditorUtility.DisplayDialog("Replace Audio Bank", "This Audio Bank already has an established location and list of assets. Changing this location will replace the current bank and erase all current audio setting for this bank. Are you sure you want to continue?", "Continue", "Cancel"))
                    {
                        return;
                    }
                }

                target.AudioBanks[j].LocationOfBankAssets = FileUtil.GetProjectRelativePath(EditorUtility.OpenFolderPanel("Choose Audio Bank Assets Location", target.AudioBanks[j].LocationOfBankAssets, string.Empty));
                UpdateAudioBank(target.AudioBanks[j]);
            }

            GUILayout.EndHorizontal();

            target.AudioBanks[j].BankOpenInEditor = EditorGUILayout.Foldout(target.AudioBanks[j].BankOpenInEditor, "Audio Bank");

            if (target.AudioBanks[j].BankOpenInEditor)
            {
                for(int k = 0; k < target.AudioBanks[j].AudioClips.Count; k++)
                {
                    if (target.AudioBanks[j].AudioClips[k].Clip == null)
                    {
                        //continue;
                    }

                    GUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(string.Format("Clip # {0}", k.ToString()), GUILayout.Width(96));

                        EditorGUILayout.Space();

                        AdjustableAudioClipEditor.StaticOnInspectorGUI(target.AudioBanks[j].AudioClips[k]);

                    GUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUILayout.EndVertical();

            EditorGUILayout.Space();
        }

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Add New Audio Bank"))
        {
            target.AudioBanks.Add(new GrendelAudioBank());
        }

        GUILayout.EndHorizontal();

        EditorGUI.indentLevel--;
        EditorGUI.indentLevel--;

		if (GrendelAudioData.PreviewAudioSource != null && GrendelAudioData.PreviewAudioSource.isPlaying)
        {

        }
    }

    public static void UpdateAudioBank(GrendelAudioBank bank)
    {
        bank.AudioClips.Clear();

		FindFiles.FileSystemEnumerator fileSystemEnumerator = new FindFiles.FileSystemEnumerator(Path.GetFullPath(bank.LocationOfBankAssets), GrendelAudioData.AcceptedAudioFileTypes, true);

        foreach(FileInfo info in fileSystemEnumerator.Matches())
        {
            AdjustableAudioClip adjustableClip = new AdjustableAudioClip();

            Object obj;

            try
            {
				Debug.Log("Looking at: " + info.DirectoryName + " Project Path: " + Path.Combine(bank.LocationOfBankAssets, info.Name) );
                obj = AssetDatabase.LoadAssetAtPath(  Path.Combine(bank.LocationOfBankAssets, info.Name), typeof(AudioClip));
            }
            catch
            {
                Debug.LogError(string.Format("No clip found at location: {0}", info.FullName));
                continue;
            }

            if (obj != null && obj.GetType() == typeof(AudioClip))
            {
                adjustableClip.Clip = obj as AudioClip;
                bank.AudioClips.Add(adjustableClip);
            }
            else
            {
                continue;
            }

        }
    }
}
