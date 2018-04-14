#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED

namespace ReflexCLI.Libraries
{
	using Attributes;

	[ConsoleCommandClassCustomizer("")]
	public static class ConsoleUtilityCommands
	{
		[ConsoleCommand]
		private static string Echo(string input)
		{
			return input;
		}

		[ConsoleCommand]
		private static void ClearHistory()
		{
			var textHandlers = UnityEngine.GameObject.FindObjectsOfType<UI.ReflexTextInputHandler>();
			for(int i=0, num=textHandlers.Length; i<num; i++)
				textHandlers[i].ClearHistory();
		}

		[ConsoleCommand]
		private static void Exit()
		{
			UI.ReflexUIManager.StaticClose();
		}

		[ConsoleCommand]
		private static string PrintCommands()
		{
			string ret = "";

			foreach(CommandDef command in CommandRegistry.GetCommands())
				ret += command.GetCommandFormat() + "\n";
			
			return ret;
		}
	}
}

#endif // REFLEXCLI_ENABLED