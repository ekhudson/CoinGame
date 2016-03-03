using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class PooledEffect : GrendelPooledObject<PooledEffect> 
{
	protected override void OnInstantiated()
	{
		mParticleSystem.Play(true);
	}
}
