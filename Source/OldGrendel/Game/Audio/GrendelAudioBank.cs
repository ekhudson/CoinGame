using UnityEngine;

using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class GrendelAudioBank
{
    public string BankName = "Audio Bank";
    public string LocationOfBankAssets = string.Empty;
    [HideInInspector]public List<AdjustableAudioClip> AudioClips = new List<AdjustableAudioClip>();

#if UNITY_EDITOR

    public bool BankOpenInEditor = false;

#endif
}
