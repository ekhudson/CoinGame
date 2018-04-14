#if(UNITY_EDITOR || DEVELOPMENT_BUILD)
#define REFLEXCLI_LIBS_AVAILABLE
#endif

#if REFLEXCLI_LIBS_AVAILABLE

using UnityEngine.SceneManagement;

namespace ReflexCLI.Libraries
{
	using Attributes;

	[ConsoleCommandClassCustomizer("Scene")]
	public static class UnitySceneCommands
	{
		[ConsoleCommand]
		private static string Reload()
		{
			return Load(SceneManager.GetActiveScene().name);
		}

		[ConsoleCommand]
		private static string Load(string sceneName)
		{
			SceneManager.LoadScene(sceneName);
			return string.Format("Loading '{0}'", sceneName);
		}
	}
}

#endif // REFLEXCLI_ENABLED
