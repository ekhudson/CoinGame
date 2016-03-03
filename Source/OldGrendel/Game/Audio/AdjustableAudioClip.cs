using UnityEngine;
using System.Collections;

[System.Serializable]
public class AdjustableAudioClip
{
    public AudioClip Clip;
    public bool RandomPitch;
    public float PitchMin = 1;
    public float PitchMax = 1;
    public float StaticPitch = 1;
	public bool Loop = false;

    [HideInInspector]public bool AttributesExpanded = false;

    public float Pitch
    {
        get
        {
            if (RandomPitch)
            {
                return Random.Range(PitchMin, PitchMax);
            }
            else
            {
                return StaticPitch;
            }
        }
        set
        {
            StaticPitch = value;
        }
    }

	public void Play()
	{

	}
}
