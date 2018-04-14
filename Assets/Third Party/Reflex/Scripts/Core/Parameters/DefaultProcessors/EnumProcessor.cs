#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED

using System;
using System.Collections.Generic;
using System.Reflection;

namespace ReflexCLI.DefaultParameterProcessors
{
	using Parameters;
	public class EnumProcessor : ParameterProcessor
	{
		public override object ConvertString(Type type, string inString)
		{
			object ret = GetMatchingNamedValue(type, inString);
			return ret != null ? ret : Enum.Parse(type, inString);
		}

		public override List<Suggestion> GetSuggestions(Type type, string subStr, object[] attributes, int maxResults)
		{
			var ret = new List<Suggestion>();

			var members = type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
			maxResults = Math.Min(members.Length, maxResults);
			for(int i=0, num=maxResults; i<num; i++)
			{
				var member = members[i];
				string enumName = member.Name;
				string enumIntVal = ((int)Enum.Parse(type, enumName)).ToString();

				if(StringStartsWith(enumName, subStr) || StringStartsWith(enumIntVal, subStr))
				{
					var display = string.Format("{0} ({1})", enumName, enumIntVal);
					ret.Add(new Suggestion(enumName, display));
				}
			}

			return ret;
		}
	}
}

#endif // REFLEXCLI_ENABLED
