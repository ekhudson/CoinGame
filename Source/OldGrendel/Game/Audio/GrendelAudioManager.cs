using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrendelAudioManager : Singleton<GrendelAudioManager> 
{
	public Dictionary<GrendelAudioChannel, Dictionary<string, List<AdjustableAudioClip>>> AudioDictionary = new Dictionary<GrendelAudioChannel, Dictionary<string, List<AdjustableAudioClip>>>();

	//private List<AdjustableAudioClip> mClipQueue = new List<AdjustableAudioClip>();

	private void Start()
	{
		EventManager.Instance.AddHandler<RequestAudioEvent>(AudioRequestHandler);
	}

	private void LateUpdate()
	{
		if (AudioDictionary.Count == 0)
		{
			GetAudioChannels();
		}
	}

	private void GetAudioChannels()
	{
		if (GameManager.Instance.ProjectData == null)
		{
			return;
		}

		foreach(GrendelAudioChannel channel in GameManager.Instance.ProjectData.AudioOptions.AudioChannels)
		{
			AudioDictionary.Add(channel, new Dictionary<string, List<AdjustableAudioClip>>());
		}
	}

	public void AudioRequestHandler(object sender, RequestAudioEvent request)
	{
		if (!AudioDictionary.ContainsKey(request.Channel))
		{
			Debug.LogWarning(string.Format("Specified Audio Channel {0}, called by {1}, does not exist!", request.Channel.ToString(), sender.ToString(), this));
		}
	
		if (!AudioDictionary[request.Channel].ContainsKey(request.Clip.ToString()))
		{
			AudioDictionary[request.Channel].Add(request.Clip.ToString(), new List<AdjustableAudioClip>(){request.Clip});
			PlayClip(request.Clip, request.Location, request.AttachToTarget, false);
		}

		if (AudioDictionary[request.Channel][request.Clip.ToString()].Count < 0)
        {
			AudioDictionary[request.Channel][request.Clip.ToString()].Add(request.Clip);
		}

	}

	public void PlayClip(AdjustableAudioClip clip, Vector3 location, Transform attachToTarget, bool forceLooping)
	{
		//AdjustableAudioClip newClip = (AdjustableAudioClip)AudioPool.Instantiate(clip, location, Quaternion.identity);

		if (attachToTarget != null)
		{

		}
	}

}
