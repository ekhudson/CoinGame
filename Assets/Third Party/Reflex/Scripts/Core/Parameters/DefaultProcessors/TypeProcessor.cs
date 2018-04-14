#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED

using System;
using System.Collections.Generic;

namespace ReflexCLI.DefaultParameterProcessors
{
	using Parameters;

	[ParameterProcessor(typeof(Type))]
	public class TypeProcessor : ParameterProcessor
	{
		public override List<Suggestion> GetSuggestions(Type inType, string subStr, object[] attributes, int maxResults)
		{
			Attributes.SubtypeOfAttribute subType = null;
			if(attributes != null)
			{
				for(int i=0, num=attributes.Length; i<num; i++)
				{
					var subTypeI = attributes[i] as Attributes.SubtypeOfAttribute;
					if(subTypeI != null)
					{
						subType = subTypeI;
						break;
					}
				}
			}

			var ret = new List<Suggestion>();

			var types = TypeRegistry.AllTypes;

			for(int i=0, num=types.Count; i<num && ret.Count < maxResults; i++)
			{
				Type type = types[i];
				if(subType != null && !subType.IsValidType(type))
					continue;

				string name = type.FullName;
				AddHierarchically(ret, name, subStr, '.');
			}

			return ret;
		}

		public override object ConvertString(Type inType, string inString)
		{
			var type = Type.GetType(inString);

			if (type != null)
				return type;

			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for(int i=0, num=assemblies.Length; i<num; i++)
			{
				type = assemblies[i].GetType(inString);
				if (type != null)
					return type;
			}

			return null;
		}
	}
}

#endif // REFLEXCLI_ENABLED
