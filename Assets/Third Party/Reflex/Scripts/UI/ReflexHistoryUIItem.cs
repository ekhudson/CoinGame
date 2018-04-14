#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

using UnityEngine;
using UnityEngine.UI;

namespace ReflexCLI.UI
{
	public class ReflexHistoryUIItem : MonoBehaviour, IUIStyleUpdater
	{
#pragma warning disable 414
		[SerializeField]
		private Text CommandText = null;

		[SerializeField]
		private Text ResultText = null;
	
		[SerializeField]
		private Text PromptMarker = null;
#pragma warning restore

#if REFLEXCLI_ENABLED
		public string Command { get { return CommandText.text; } }

		public void Setup(string commandStr, string resultStr)
		{
			gameObject.name = commandStr;
			CommandText.text = commandStr;
			DoResults(resultStr);
			((IUIStyleUpdater)this).UpdateStyles();
		}

		private void DoResults(string resultStr)
		{
			if(string.IsNullOrEmpty(resultStr))
			{
				ResultText.gameObject.SetActive(false);
			}
			else if(resultStr.Contains("\n"))
			{
				var resultStrings = resultStr.Split('\n');
				var parent = ResultText.transform.parent;
				for(int i=0, num=resultStrings.Length; i<num; i++)
				{
					var lineStr = resultStrings[i];
					var lineText = Instantiate(ResultText);
					lineText.transform.SetParent(parent,false);
					lineText.text = lineStr;
				}
				ResultText.gameObject.SetActive(false);
			}
			else
			{
				ResultText.text = resultStr;
			}
		}

		public void SetupLog(string logString, string stackTrace, LogType type)
		{
			gameObject.name = type.ToString();
			CommandText.text = logString;
			CommandText.color = GetLogColor(type);
			ResultText.gameObject.SetActive(false);
			PromptMarker.text = "";
		}

		private static Color GetLogColor(LogType type)
		{
			switch(type)
			{
			case LogType.Error:
			case LogType.Assert:
			case LogType.Exception:
				return new Color(0.8f,0.0f,0.0f);// "#cc0000ff";
			case LogType.Warning:
				return new Color(0.9f,0.9f,0.0f); //"#eeee00ff";
			default:
				return new Color(0.8f,0.8f, 0.8f); //"#ccccccff";
			}
		}

		void IUIStyleUpdater.UpdateStyles()
		{
			var font = Settings.ConsoleFont;
			if(font)
			{
				var textComponents = transform.GetComponentsInChildren<Text>();
				for(int i=0, num=textComponents.Length; i<num; i++)
					textComponents[i].font = font;
			}
		}
#else
		void IUIStyleUpdater.UpdateStyles(){}
#endif // REFLEXCLI_ENABLED

	}
}