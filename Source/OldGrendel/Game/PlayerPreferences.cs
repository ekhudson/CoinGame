using UnityEngine;
using System.Collections;

public static class PlayerPreferences
{

#region SOUND

    //Music Volume
    private const string kMusicVolumeKey = "MusicVolumeKey";
    private const float kDefaultMusicVolume = 1;

    public static float MusicVolume
    {
        get { return PlayerPrefs.GetFloat(kMusicVolumeKey, kDefaultMusicVolume); } set { PlayerPrefs.SetFloat(kMusicVolumeKey, value); }
    }

    //Sound Volume
    private const string kSFXVolumeKey = "SFXVolumeKey";
    private const float kDefaultSFXVolume = 1;

    public static float SFXVolume
    {
        get { return PlayerPrefs.GetFloat(kSFXVolumeKey, kDefaultSFXVolume); } set { PlayerPrefs.SetFloat(kSFXVolumeKey, value); }
    }

#endregion

}
