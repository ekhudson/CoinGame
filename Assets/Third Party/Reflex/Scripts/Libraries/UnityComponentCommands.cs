#if(UNITY_EDITOR || DEVELOPMENT_BUILD)
#define REFLEXCLI_LIBS_AVAILABLE
#endif

#if REFLEXCLI_LIBS_AVAILABLE

using System;
using System.Collections.Generic;
using UnityEngine;


namespace ReflexCLI.Libraries
{
	using Attributes;

	[ConsoleCommandClassCustomizer("Component")]
	public static class UnityComponentCommands
	{
		[ConsoleCommand]
		private static string FindInScenes([SubtypeOf(typeof(Component), includeInterfaces:true)] Type type, bool includeInactive=true)
		{
			var validType = typeof(Component);
			if(type == null || (type != validType && !type.IsSubclassOf(validType) && !type.IsInterface))
				throw (new CommandException("Invalid type '{0}' - must be a subclass of {1}", type != null ? type.FullName :"null", validType));

			UnityEngine.Object comp = Utils.FindObjectOfType(type, includeInactive);
			if(!comp)
				throw (new CommandException("Could Not find any objects of type '{0}'", type));

			return GetComponentPath(comp as Component);
		}

		[ConsoleCommand]
		private static string FindAllInScenes([SubtypeOf(typeof(Component), includeInterfaces:true)] Type type, bool includeInactive=true)
		{
			var validType = typeof(Component);
			if(type == null || (type != validType && !type.IsSubclassOf(validType) && !type.IsInterface))
				throw (new CommandException("Invalid type '{0}' - must be a subclass of {1}", type != null ? type.FullName :"null", validType));

			var allComps = Utils.FindObjectsOfType(type, includeInactive);
			if(allComps.Count == 0)
				throw (new CommandException("Could Not find any objects of type '{0}'", type));

			var str = "";
			for(int i=0, num=allComps.Count; i<num; i++)
				str += GetComponentPath(allComps[i] as Component) + "\n";

			return str.TrimEnd('\n');
		}

		private static string GetComponentPath(Component component)
		{
			string ret = "";
			var trans = component.transform;
			do
			{
				ret = "/" + trans.name + ret;
				var parent = trans.parent;
				if(!parent)
					ret = "/" + trans.gameObject.scene.name + ret;

				trans = trans.parent;
			} while(trans);

			return ret;
		}
	}
}

#endif // REFLEXCLI_LIBS_AVAILABLE
