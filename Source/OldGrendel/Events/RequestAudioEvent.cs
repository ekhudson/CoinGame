using UnityEngine;
using System.Collections;

public class RequestAudioEvent : EventBase 
{
	public readonly GrendelAudioChannel Channel;
	public readonly AdjustableAudioClip Clip;
	public readonly Transform AttachToTarget = null;
	public readonly Vector3 Location = Vector3.zero;
	public readonly bool ForceLooping = false;
	public readonly float Volume = 0f;

	public RequestAudioEvent(GrendelAudioChannel channel, AdjustableAudioClip audioClip, Vector3 location, Transform attachToTarget, float volume, bool forceLooping, object sender) : base (Vector3.zero, sender)
	{
		Channel = channel;
		Clip = audioClip;
		AttachToTarget = attachToTarget;
		Location = location;
		ForceLooping = forceLooping;
		Volume = volume;
	}

	public RequestAudioEvent(GrendelAudioChannel channel, AdjustableAudioClip audioClip, Vector3 location, Transform attachToTarget, object sender) : base (Vector3.zero, sender)
	{
		Channel = channel;
		Clip = audioClip;
		Location = location;
		AttachToTarget = attachToTarget;
	}

	public RequestAudioEvent(GrendelAudioChannel channel, AdjustableAudioClip audioClip, Vector3 location, object sender) : base (Vector3.zero, sender)
	{
		Channel = channel;
		Clip = audioClip;
		Location = location;
	}
}
