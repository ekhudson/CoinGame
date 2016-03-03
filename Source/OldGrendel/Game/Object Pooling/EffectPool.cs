using UnityEngine;
using System.Collections;

public class EffectPool : GrendelObjectPool<PooledEffect> 
{
	protected override void CopyOriginal (PooledEffect obj, PooledEffect original)
	{
		base.CopyOriginal (obj, original);
	}
}
