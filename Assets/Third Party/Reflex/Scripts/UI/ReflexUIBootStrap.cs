#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace ReflexCLI.UI
{
	class BootStrap
	{
#if REFLEXCLI_DEVELOPMENT && UNITY_EDITOR
		[UnityEditor.InitializeOnLoadMethod]
		private static void DevModeTracker()
		{
			Debug.Log("Editor running With REFLEXCLI_DEVELOPMENT flag set");
		}
#endif

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void CreateUI()
		{
			Object canvas = null;
			
#if REFLEXCLI_DEVELOPMENT && UNITY_EDITOR
			canvas = GameObject.FindObjectOfType<ReflexUIManager>();
#endif
			if(!canvas)
				canvas = Instantiate(Settings.CanvasPrefabName);

			if(canvas)
				Object.DontDestroyOnLoad(canvas);

			CreateEventSystemIfNeeded(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
			SceneManager.activeSceneChanged += CreateEventSystemIfNeeded;		
		}


		private static void CreateEventSystemIfNeeded(Scene a, Scene b)
		{
			if(!EventSystem.current)
				Instantiate(Settings.EventSystemPrefabName);
		}

		private static Object Instantiate(string resourceName)
		{
			var prefab = Resources.Load(resourceName);
			if(prefab)
			{
				var go = GameObject.Instantiate(prefab);
				if(go)
					go.name = resourceName;
				return go;
			}
			return null;
		}
	}
}
#endif // REFLEXCLI_ENABLED