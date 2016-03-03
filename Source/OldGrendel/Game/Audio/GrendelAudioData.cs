using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class GrendelAudioEntry
{
	public int AudioBankNumber = 0;
	public int AudioClipNumber = 0;
}

[System.Serializable]
public class GrendelAudioData
{
    [HideInInspector]public List<GrendelAudioChannel> AudioChannels = new List<GrendelAudioChannel>();
    [HideInInspector]public List<GrendelAudioBank> AudioBanks = new List<GrendelAudioBank>();

    public const string AcceptedAudioFileTypes = "*.wav;*.mp3;*.ogg";

    private static AudioSource mPreviewAudioSource;

    public static AudioSource PreviewAudioSource
    {
        get
        {
            return mPreviewAudioSource;
        }
    }

    public static void PlayAudioClipPreview(AdjustableAudioClip clip)
    {
        if (mPreviewAudioSource == null)
        {
            mPreviewAudioSource = new GameObject("_previewAudioClip", typeof(AudioSource)).GetComponent<AudioSource>();
            mPreviewAudioSource.gameObject.hideFlags = HideFlags.HideAndDontSave;
        }
        else if (mPreviewAudioSource.isPlaying && mPreviewAudioSource.clip == clip.Clip)
        {
            mPreviewAudioSource.Stop();
            return;
        }

		if (Application.isPlaying)
		{
			mPreviewAudioSource.gameObject.transform.parent = Camera.main.transform;
			mPreviewAudioSource.gameObject.transform.localPosition = Vector3.zero;
		}
		else
		{
        	mPreviewAudioSource.gameObject.transform.position = Vector3.zero;
			mPreviewAudioSource.maxDistance = 1000000f;
		}

        mPreviewAudioSource.clip = clip.Clip;
        mPreviewAudioSource.pitch = clip.Pitch;
        mPreviewAudioSource.Play();;
    }
}
