using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(GrendelProjectData))]
public class GrendelProjectDataEditor : GrendelEditor<GrendelProjectData>
{
    private const float kDeleteButtonWidth = 48f;

    public static GrendelProjectData CreateProjectDataAsset()
    {
        GrendelProjectData asset = (GrendelProjectData)ScriptableObject.CreateInstance(typeof(GrendelProjectData));  //scriptable object
        if (!Directory.Exists(Path.Combine(Application.dataPath,"Resources\\Grendel\\")))
        {
            Directory.CreateDirectory(Path.Combine(Application.dataPath,"Resources\\Grendel\\"));
        }

        AssetDatabase.CreateAsset(asset, string.Format("Assets/Resources/Grendel/{0}.asset", "Grendel Project Data"));
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

        return asset;
    }

    public override void OnInspectorGUI()
    {
        SerializedObject serializedObject = new SerializedObject(Target);

        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();

        //GrendelAudioDataEditor.StaticOnInspectorGUI(Target.AudioOptions);

        if(EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(Target);
            serializedObject.ApplyModifiedProperties();
        }
    }

    public void DrawAudioOptions()
    {

    }

//    public void UpdateAudioBank(GrendelAudioBank bank)
//    {
//        bank.AudioClips.Clear();
//
//        FindFiles.FileSystemEnumerator fileSystemEnumerator = new FindFiles.FileSystemEnumerator(bank.LocationOfBankAssets, GrendelAudioOptions.AcceptedAudioFileTypes, true);
//
//        foreach(FileInfo info in fileSystemEnumerator.Matches())
//        {
//            AdjustableAudioClip adjustableClip = new AdjustableAudioClip();
//
//            Object obj;
//
//            try
//            {
//                Debug.Log("Looking at: " + info.FullName + " Project Path: " + FileUtil.GetProjectRelativePath( info.ToString() ));
//                obj = AssetDatabase.LoadAssetAtPath( FileUtil.GetProjectRelativePath( info.ToString() ) , typeof(AudioClip));
//            }
//            catch
//            {
//                Debug.LogError(string.Format("No clip found at location: {0}", info.FullName));
//                continue;
//            }
//
//            if (obj != null && obj.GetType() == typeof(AudioClip))
//            {
//                adjustableClip.Clip = obj as AudioClip;
//                bank.AudioClips.Add(adjustableClip);
//            }
//            else
//            {
//                continue;
//            }
//
//        }
//    }
}
