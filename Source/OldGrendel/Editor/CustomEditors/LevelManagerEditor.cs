using System.Collections;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : GrendelEditor<LevelManager>
{
    //public AudioList TheAudioList;
   // public string[] MusicTracks = new string[0];

    // Override the GUI
    public override void OnInspectorGUI()
    {   
		DrawDefaultInspector();
//        if (Application.isPlaying) { return; }
//
//        //TheAudioList = (AudioList)GameObject.FindObjectOfType(typeof(AudioList));
//        MusicTracks = new string[TheAudioList.MusicTracks.Count];        
//            
//        int i = 0;
//
//        foreach(AudioClip clip in TheAudioList.MusicTracks)
//        {            
//            MusicTracks[i] = clip.name;
//            i++;
//        }
//
//        EditorGUI.BeginChangeCheck();
//        
//            Target.RandomMusicTrack = EditorGUILayout.Toggle("Random Music Track:", Target.RandomMusicTrack);
//
//            EditorGUI.BeginDisabledGroup (Target.RandomMusicTrack == true);
//
//            Target.MusicTrackIndex = EditorGUILayout.Popup( "Background Music:", Target.MusicTrackIndex, MusicTracks);
//            
//            EditorGUI.EndDisabledGroup();
//
//        if(EditorGUI.EndChangeCheck())
//        {
//            EditorUtility.SetDirty(Target);
//        }
//        
//        if (TheAudioList.MusicTracks.Count > 0)
//        {
//            Target.BackgroundMusicTrack = TheAudioList.MusicTracks[Target.MusicTrackIndex];
//        }
    }
}

