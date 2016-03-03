using UnityEngine;
using System.Collections;

public class AudioPool : GrendelObjectPool<PooledAudio>  
{
	protected override void CopyOriginal (PooledAudio obj, PooledAudio original)
	{
		base.CopyOriginal (obj, original);
	}
}
