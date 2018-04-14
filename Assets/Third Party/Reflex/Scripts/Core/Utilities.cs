#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ReflexCLI
{
	public static class Utils
	{
		private static char[] _CommandDelimeters = new char[]{' '};
		public static char[] CommandDelimeters {get {return _CommandDelimeters; } }

		static Regex DuplicateWhitespaceRegex = new Regex("[ ]{2,}", RegexOptions.None);

		// Helper functions:

		public static string[] GetCommandTerms(string commandStr)
		{
			if(string.IsNullOrEmpty(commandStr))
				return new string[0];

			commandStr = DuplicateWhitespaceRegex.Replace(commandStr, " ");
			return commandStr.Split(CommandDelimeters);
		}

		public static string GetCommand(string[] terms)
		{
			var ret = "";
			for(int i=0, num=terms.Length; i<num; i++)
				ret += terms[i] + " ";

			return ret;
		}

		public static object ExecuteCommand(ref string commandStr)
		{
			string[] commandTerms = GetCommandTerms(commandStr);
			if(commandTerms.Length == 0)
				throw(new CommandException("Empty Command passed '{0}'", commandStr));

			CommandDef commandDef = CommandRegistry.GetCommand(commandTerms[0]);
			if(!commandDef.IsValid())
				throw(new CommandException("Unrecognised Command '{0}'", commandTerms[0]));

			commandStr = FixCommandFormat(commandTerms, commandDef);

			return commandDef.Command.ExecuteCommand(commandTerms);
		}

		private static string FixCommandFormat(string[] commandTerms, CommandDef commandDef)
		{
			string ret = commandDef.Name;

			int numTerms = Mathf.Min(commandTerms.Length, commandDef.Command.GetNumExpectedTerms());
			for(int i=1; i<numTerms; i++)
				ret += " " + commandTerms[i];

			return ret;
		}

		public static bool StringStartsWith(string inString, string subStr)
		{
			return inString.StartsWith(subStr, Settings.SuggestionsCaseInsensitive, System.Globalization.CultureInfo.CurrentCulture);
		}

		public static bool StringEquals(string inString, string candidate)
		{
			StringComparison comparison = Settings.SuggestionsCaseInsensitive ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;
			return inString.Equals(candidate, comparison);
		}

		public static UnityEngine.Object FindObjectOfType(Type type, bool includeInactive)
		{
			if(!includeInactive && !type.IsInterface)
				return GameObject.FindObjectOfType(type);

			var scenes = GetAllScenes();
			for(int s=0, numS=scenes.Length; s<numS; s++)
			{
				var scene = scenes[s];
				var roots = scene.GetRootGameObjects();

				if(type == typeof(GameObject) && roots.Length > 0)
					return roots[0].gameObject;

				for(int r=0, numR=roots.Length; r<numR; r++)
				{
					var ret = roots[r].transform.GetComponentInChildren(type, includeInactive);
					if(ret)
						return ret;
				}
			}
			return null;
		}

		public static List<UnityEngine.Object> FindObjectsOfType(Type type, bool includeInactive)
		{
			List<UnityEngine.Object> ret = new List<UnityEngine.Object>();
			if(!includeInactive && !type.IsInterface)
			{
				ret.AddRange(GameObject.FindObjectsOfType(type));
				return ret;
			}

			var scenes = GetAllScenes();
			for(int s=0, numS=scenes.Length; s<numS; s++)
			{
				var scene = scenes[s];
				var roots = scene.GetRootGameObjects();
				for(int r=0, numR=roots.Length; r<numR; r++)
				{
					var root = roots[r].transform;
					if(type == typeof(GameObject))
					{
						var transforms = root.GetComponentsInChildren<Transform>();
						for(int t=0, numT=transforms.Length; t<numT; t++)
							ret.Add(transforms[t].gameObject);
					}
					else
					{
						ret.AddRange(root.GetComponentsInChildren(type, includeInactive));
					}
				}
			}
			return ret;
		}

		private static Scene DDOLScene = new Scene();
		private static Scene[] GetAllScenes()
		{
			var ret = new Scene[SceneManager.sceneCount + 1];
			for(int s=0, numS=SceneManager.sceneCount; s<numS; s++)
				ret[s] = SceneManager.GetSceneAt(s);

			// Scene manager won't tell us about DontDestroyOnLoad scenes
			if(!DDOLScene.isLoaded)
			{
				var go=new GameObject();
				GameObject.DontDestroyOnLoad(go);
				DDOLScene = go.scene;
				GameObject.Destroy(go);
			}

			ret[SceneManager.sceneCount] = DDOLScene;
			return ret;
		}
	}
}

#endif // REFLEXCLI_ENABLED
