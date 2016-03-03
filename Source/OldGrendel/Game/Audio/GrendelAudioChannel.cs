using UnityEngine;
using System.Collections;

[System.Serializable]
public class GrendelAudioChannel
{
    public string ChannelName = "Audio Channel";
	public int MaxSimultaneousSounds = 50;

	private float mChannelVolume = 1f;

    public float ChannelVolume
    {
        get
        {
            return mChannelVolume;
        }
        set
        {
            mChannelVolume = Mathf.Clamp(value, 0f, 1f);
        }
    }
}
