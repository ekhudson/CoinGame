#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED

using System;
using System.Collections.Generic;

namespace ReflexCLI.DefaultParameterProcessors
{
	using Parameters;

	[ParameterProcessor(typeof(CommandDef))]
	public class CommandDefProcessor : ParameterProcessor
	{
		public override List<Suggestion> GetSuggestions(Type type, string subStr, object[] attributes, int maxResults)
		{
			List<Suggestion> ret = new List<Suggestion>();
			foreach(CommandDef def in CommandRegistry.GetCommands())
			{
				AddHierarchically(ret, def.Name, subStr, '.');
				if(ret.Count == maxResults)
					break;
			}

			return ret;
		}

		public override object ConvertString(Type type, string inString)
		{
			foreach(CommandDef def in CommandRegistry.GetCommands())
			{
				if(StringEquals(inString, def.Name))
					return def;
			}

			return new CommandDef();
		}
	}
}

#endif // REFLEXCLI_ENABLED