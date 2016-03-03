using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioClip))]
public class PooledAudio : GrendelPooledObject<PooledAudio> 
{
	protected override void OnInstantiated()
	{

	}
}
