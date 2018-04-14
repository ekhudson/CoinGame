#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ReflexCLI.UI
{
	public class ReflexHistoryUIContainer : MonoBehaviour
	{
		[SerializeField]
		private ScrollRect Scroller;

		[SerializeField]
		private RectTransform ItemParent;

		[SerializeField]
		private ReflexHistoryUIItem ItemTemplate;

#if REFLEXCLI_ENABLED
		List<string> Entries = new List<string>();

		public void Init()
		{
			Application.logMessageReceived -= HandleLog;
			Application.logMessageReceived += HandleLog;
		}

		public void AddItem(string commandStr, string resultStr)
		{
			var newItem = SpawnItem();
			newItem.Setup(commandStr, resultStr);

			Entries.Insert(0, commandStr);
			ResetScroll();
		}
		
		public void Clear()
		{
			foreach(Transform child in ItemParent)
				Destroy(child.gameObject);
		}

		public void ResetScroll()
		{
			Scroller.normalizedPosition = new Vector2(0.0f, 0.0f);
		}

		public int GetNumEntries()
		{
			return Entries.Count;
		}

		public string GetEntry(int idx)
		{
			Assert.IsTrue(idx >= 0);
			Assert.IsTrue(idx < GetNumEntries());
			
			return Entries[idx];
		}

		private ReflexHistoryUIItem SpawnItem()
		{
			var newItem = Instantiate<ReflexHistoryUIItem>(ItemTemplate);
			newItem.transform.SetParent(ItemParent, false);
			return newItem;
		}

		private void HandleLog(string logString, string stackTrace, LogType type)
		{
			if(Settings.DisplayUnityLog)
			{
				var newItem = SpawnItem();
				newItem.SetupLog(logString, stackTrace, type);
			}
	    }
#endif //REFLEXCLI_ENABLED
	}
}