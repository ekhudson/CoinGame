#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

using System;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

namespace ReflexCLI.UI
{
	public class ReflexUIManager : MonoBehaviour
	{
		public static event Action OnConsoleOpened = delegate {};
		public static event Action OnConsoleClosed = delegate {};
		
#if REFLEXCLI_ENABLED
		private bool wantClose = false;

		private static ReflexUIManager Instance;

		public static bool IsConsoleOpen()
		{
			return ConsoleDisplay && ConsoleDisplay.activeInHierarchy;
		}

		public static void StaticOpen()
		{
			if(Instance)
				Instance.OpenConsole();
		}

		public static void StaticClose()
		{
			if(Instance)
				Instance.CloseConsole();
		}

		private static GameObject ConsoleDisplay = null;

		void Awake()
		{
			Assert.IsNull(ConsoleDisplay);

			Instance = this;

			var displayTrans = transform.GetChild(0);
			ConsoleDisplay = displayTrans ? displayTrans.gameObject : null;

			Assert.IsNotNull(ConsoleDisplay);

			var inputHandler = displayTrans.GetComponentInChildren<ReflexTextInputHandler>(true);
			inputHandler.Init();

			ConsoleDisplay.SetActive(false);
			UpdateVisualStyle();
#if UNITY_EDITOR
			Settings.OnConsoleVisualsChanged += UpdateVisualStyle;
			Settings.OnConsoleDisabled += CloseConsole;
#endif
		}

		private void OnEnable()
		{
			StartCoroutine(CloseConsoleAtEOF());
		}

		private void OnDisable()
		{
			StopAllCoroutines();
		}
		
		private void OnDestroy()
		{
#if UNITY_EDITOR
			Settings.OnConsoleVisualsChanged -= UpdateVisualStyle;
			Settings.OnConsoleDisabled -= CloseConsole;
#endif
		}
		
		void Update()
		{
			if(!ConsoleDisplay)
				return;

			if(IsConsoleOpen())
			{
				if(Input.GetKeyDown(Settings.CloseConsoleKey))
					CloseConsole();
			}
			else if(Input.GetKeyDown(Settings.OpenConsoleKey))
			{
				OpenConsole();
			}
		}

		public void OpenConsole()
		{
			if(!IsConsoleOpen() && ConsoleDisplay && Settings.EnableConsole)
			{
				ConsoleDisplay.SetActive(true);
				OnConsoleOpened();
			}
		}

		public void CloseConsole()
		{
			wantClose = true;
		}

		private IEnumerator CloseConsoleAtEOF()
		{
			var waitEOF = new WaitForEndOfFrame();
			while(true)
			{
				yield return waitEOF;

				if(wantClose && IsConsoleOpen())
				{
					ConsoleDisplay.SetActive(false);
					OnConsoleClosed();
					wantClose = false;
				}
			}
		}

		private void UpdateVisualStyle()
		{
			var stylers = transform.GetComponentsInChildren<IUIStyleUpdater>(true);
			for(int i=0, num=stylers.Length; i<num; i++)
				stylers[i].UpdateStyles();
		}

#else // !REFLEXCLI_ENABLED
		void Awake()
		{
			GameObject.Destroy(gameObject);
		}

		public static bool IsConsoleOpen()
		{
			return false;
		}

		public static void StaticOpen(){}
		public static void StaticClose(){}
#endif //REFLEXCLI_ENABLED
	}
}