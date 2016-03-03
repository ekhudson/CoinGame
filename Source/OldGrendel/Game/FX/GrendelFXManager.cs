using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrendelFXManager : Singleton<GrendelFXManager> 
{
	public class FXLibaryEntry
	{
		public string EntryName = string.Empty;
		public GameObject EntryPrefab;
	}

	public EffectPool FXPool;

	[HideInInspector]public string FXLibraryPath;

	public List<FXLibaryEntry> FXLibrary;

	private Dictionary<string, GameObject> mFXLibraryDict = new Dictionary<string, GameObject>();

	private void Start()
	{
		if (FXPool == null)
		{
			Debug.LogWarning("FX Pool not specified; FX pooling will not occur");
		}

		mFXLibraryDict.Clear();

		if (FXLibrary == null || FXLibrary.Count == 0)
		{
			return;
		}

		foreach(FXLibaryEntry entry in FXLibrary)
		{
			mFXLibraryDict.Add(entry.EntryName, entry.EntryPrefab);
		}

	}

	public BaseObject SpawnEffect(GameObject effect, Vector3 position, Quaternion rotation, bool pooledOnly)
	{
		return FXPool.Instantiate(effect, position, rotation, pooledOnly) as BaseObject;
	}

	public BaseObject SpawnEffectFromLibrary(string effectName, Vector3 position, Quaternion rotation, bool pooledOnly)
	{
		if (mFXLibraryDict[effectName] == null)
		{
			Debug.LogError(string.Format("FX Library contains no effect with name: {0}", effectName), this);
			return null;
		}

		return SpawnEffect(mFXLibraryDict[effectName], position, rotation, pooledOnly);
	}

}
