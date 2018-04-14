using System;
using UnityEngine;

namespace ReflexCLI.User
{
	using UI;

	public static class ConsoleStatus
	{
		public static event Action OnConsoleOpened = delegate {};
		public static event Action OnConsoleClosed = delegate {};

		public static bool IsConsoleOpen()
		{
			return ReflexUIManager.IsConsoleOpen();
		}

		public static void OpenConsole()
		{
			ReflexUIManager.StaticOpen();
		}

		public static void CloseConsole()
		{
			ReflexUIManager.StaticClose();
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			ReflexUIManager.OnConsoleOpened += OnConsoleOpened;
			ReflexUIManager.OnConsoleClosed += OnConsoleClosed;
		}
	}
}