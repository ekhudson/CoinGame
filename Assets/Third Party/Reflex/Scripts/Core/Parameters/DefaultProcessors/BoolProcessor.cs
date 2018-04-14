#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED

using System;
using System.Collections.Generic;

namespace ReflexCLI.DefaultParameterProcessors
{
	using Parameters;

	[ParameterProcessor(typeof(bool))]
	public class BoolProcessor : ParameterProcessor
	{
		private string[] suggestions = {"True", "False"};

		public override List<Suggestion> GetSuggestions(Type type, string subStr, object[] attributes, int maxResults)
		{
			return GetMatchingSuggestions(suggestions, subStr, maxResults);
		}
	}
}

#endif // REFLEXCLI_ENABLED
