using UnityEngine;
using System.Collections;

public class GrendelPooledObject<T> : BaseObject where T : MonoBehaviour
{
	private GrendelObjectPool<T> mParentPool;

	public GrendelObjectPool<T> ParentPool
	{
		get
		{
			return mParentPool;
		}
		set
		{
			mParentPool = value;
		}
	}

	protected virtual void OnInstantiated()
	{
		//override for special behavior when "instantiated" from a pool
	}

	protected void Pool()
	{
		mParentPool.AddToPool(this);
		this.OnPooled();
	}

	protected virtual void OnPooled()
	{
		//override for special behaviour when added back to the pool
	}
}
