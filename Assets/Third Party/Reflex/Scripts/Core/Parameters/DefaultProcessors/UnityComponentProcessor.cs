#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ReflexCLI.DefaultParameterProcessors
{
	using Parameters;
	
	[ParameterProcessor(typeof(GameObject))]
	[ParameterProcessor(typeof(Component))]
	public class UnityComponentProcessor : ParameterProcessor
	{
		public override List<Suggestion> GetSuggestions(Type type, string subStr, object[] attributes, int maxResults)
		{
			var ret = new List<Suggestion>();

			subStr = subStr.Trim('"');

			var instances = FindObjectsOfType(type);
			for(int i=0, num=instances.Count; i<num; i++)
			{
				var name = instances[i].name.Replace(' ', '_');
				if(StringStartsWith(name, subStr))
				{
					ret.Add(name);
				}
			}
			return ret;
		}

		public override object ConvertString(Type type, string inString)
		{
			var objects = FindObjectsOfType(type);
			for(int i=0, num=objects.Count; i<num; i++)
			{
				var obj = objects[i];
				if(obj.name.Replace(' ', '_') == inString)
					return obj;
			}

			return null;
		}

		private List<UnityEngine.Object> FindObjectsOfType(Type type)
		{
			return Utils.FindObjectsOfType(type, Settings.IncludeInactiveObjects);
		}
	}
}

#endif //REFLEXCLI_ENABLED
