#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED

using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace ReflexCLI
{
	public class Settings : ScriptableObject
	{
		public const string CanvasPrefabName = "ReflexConsoleCanvas";
		public const string EventSystemPrefabName = "ReflexConsoleEventSystem";

		private const string AssetName = "ReflexConsoleSettings";
		private const string MenuPath = "Edit/Project Settings/Reflex/Console Settings";
		[Header("General")]
		[SerializeField, Tooltip("Global enable control for the console (cannot be changed at runtime)")]
		private bool _EnableConsole = true;
		public static bool EnableConsole { get { return Instance._EnableConsole; } }

		[Header("Control Keys")]
		[SerializeField, Tooltip("KeyCode for opening the console")]
		private KeyCode _OpenConsoleKey = KeyCode.Tab;
		public static KeyCode OpenConsoleKey { get { return Instance._OpenConsoleKey; } }

		[SerializeField, Tooltip("KeyCode for closing the console")]
		private KeyCode _CloseConsoleKey = KeyCode.Escape;
		public static KeyCode CloseConsoleKey { get { return Instance._CloseConsoleKey; } }

		[Header("Logging")]
		[SerializeField, Tooltip("Displays standard Unity Debug.Log() Messages in the console")]
		private bool _DisplayUnityLog = false;
		public static bool DisplayUnityLog {get {return Instance._DisplayUnityLog; } }
		
		[Header("Command Format")]
		[SerializeField, Tooltip("Define the default naming for Commands")]
		private ENamingConvention _DefaultNamingConvention = ENamingConvention.NamespaceAndClassPrefix;
		public static ENamingConvention DefaultNamingConvention {get {return  Instance._DefaultNamingConvention; } }

		[SerializeField, Tooltip("Ignore case for autocomplete suggestions")]
		private bool _SuggestionsCaseInsensitive = true;
		public static bool SuggestionsCaseInsensitive {get {return Instance._SuggestionsCaseInsensitive; } }

		[SerializeField, Tooltip("Autocomplete using hierarchies (i.e. group by namespace/classname etc.)")]
		private bool _HierarchicalAutocomplete = true;
		public static bool HierarchicalAutocomplete {get {return Instance._HierarchicalAutocomplete; } }

		[Header("Console Behaviour")]
		[SerializeField, Tooltip("Should console search for inactive GameObjects / MonoBehaviours when processing command parameters?")]
		private bool _IncludeInactiveObjects = true;
		public static bool IncludeInactiveObjects {get {return Instance._IncludeInactiveObjects; } }

		[Header("Console Visuals")]
		[SerializeField, Tooltip("Set Font Overrides")]
		private Font _ConsoleFont;
		public static Font ConsoleFont {get {return Instance._ConsoleFont ? Instance._ConsoleFont : defaultFont; } }

		private static Font defaultFont = null;
#if UNITY_EDITOR
		private bool PrevEnableConsole = false;
		private ENamingConvention PrevNamingConvention = 0;
		private Font PrevFont = null;
		void OnEnable()
		{
			PrevEnableConsole = _EnableConsole;
			PrevNamingConvention = _DefaultNamingConvention;
			PrevFont = _ConsoleFont;

			defaultFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
		}

		public static event Action OnConsoleEnabled = delegate {};
		public static event Action OnConsoleDisabled = delegate {};
		public static event Action OnConsoleVisualsChanged = delegate {};

		void OnValidate()
		{
			_Instance = this;

			if(PrevEnableConsole && !_EnableConsole)
				OnConsoleDisabled();
			else if(!PrevEnableConsole && _EnableConsole)
				OnConsoleEnabled();

			if(PrevNamingConvention != _DefaultNamingConvention)
				CommandRegistry.Rebuild();

			if(PrevFont != _ConsoleFont)
				OnConsoleVisualsChanged();

			PrevEnableConsole = _EnableConsole;
			PrevNamingConvention = _DefaultNamingConvention;
			PrevFont = _ConsoleFont;
		}
#endif
		   
		/////////////////////////////////////////////////////////////////////////////////////////
		// Project Settings implementation

		static private Settings _Instance = null;
		static public Settings Instance { get { return GetOrCreateInstance(); } }


		static public Settings GetOrCreateInstance()
		{
			if(_Instance)
				return _Instance;

			_Instance = Resources.Load<Settings>(AssetName);

			if(!_Instance)
			{
				var asset = ScriptableObject.CreateInstance<Settings>();

#if UNITY_EDITOR
				var canvas = Resources.Load(CanvasPrefabName);
				if(canvas)
				{
					var canvasPath = UnityEditor.AssetDatabase.GetAssetPath(canvas);
					var canvasDir = System.IO.Path.GetDirectoryName(canvasPath);
			
					UnityEditor.AssetDatabase.CreateAsset(asset, string.Concat(canvasDir, "/", AssetName, ".asset"));
				}
				else
				{
					// editor probably building asset database, try again on the next editor tick
					UnityEditor.EditorApplication.update += CreateSettings;
				}
#endif

				_Instance = asset;
			}

			Assert.IsNotNull(_Instance);
			return _Instance;
		}

#if UNITY_EDITOR
		[UnityEditor.MenuItem(MenuPath)]
		private static void OpenAsset()
		{
			UnityEditor.Selection.activeObject = Instance;
		}

		[UnityEditor.InitializeOnLoadMethod]
		private static void CreateSettings()
		{
			UnityEditor.EditorApplication.update -= CreateSettings;
			GetOrCreateInstance();
		}
#endif
	}
}

#endif // REFLEXCLI_ENABLED
