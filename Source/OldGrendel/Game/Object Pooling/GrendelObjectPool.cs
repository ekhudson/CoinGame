using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class GrendelObjectPool<T> : BaseObject where T : MonoBehaviour
{
	public int DefaultInstanceCount = 4;
	public int MaximumAllowedInstances = 100;

	private List<T> PoolList = new List<T>();

	protected override void Start()
	{
		transform.position = Vector3.zero;

		T obj;

		for(int i = 0; i < DefaultInstanceCount; i++)
		{ 
			obj = new GameObject("~" + typeof(T).ToString(), typeof(T)).GetComponent<T>();
			obj.transform.parent = transform;
			obj.transform.position = Vector3.zero;
			obj.GetComponent<GrendelPooledObject<T>>().ParentPool = this;
			obj.gameObject.SetActive(false);
			PoolList.Add(obj);
		}
	}

	public virtual T Instantiate(GameObject original, Vector3 position, Quaternion rotation, bool pooledOnly)
	{
		T obj;

		if (PoolList.Count == 0 && !pooledOnly)
		{
			obj = (T)Object.Instantiate(original);
			obj.transform.parent = mTransform;
			obj.transform.position = Vector3.zero;
			obj.GetComponent<GrendelPooledObject<T>>().ParentPool = this;

		}
		else if (PoolList.Count > 0 && mTransform.childCount < MaximumAllowedInstances)
		{
			obj = PoolList[PoolList.Count - 1];
			CopyOriginal(obj, original.GetComponent<T>());
			obj.gameObject.SetActive(true);
			PoolList.RemoveAt(PoolList.Count - 1);
		}
		else
		{
			return null;
		}

		obj.transform.position = position;
		obj.transform.rotation = rotation;

		return obj;
	}

	protected virtual void CopyOriginal(T obj, T original)
	{
		//override for specific behaviour
		obj.name = "~" + original.name;
	}

	public void AddToPool(GrendelPooledObject<T> objectToPool)
	{
		objectToPool.gameObject.SetActive(false);
		PoolList.Add(objectToPool as T);
	}

}
