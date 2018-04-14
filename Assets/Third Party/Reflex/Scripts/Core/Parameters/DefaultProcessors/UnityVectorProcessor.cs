#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED

using System;
using UnityEngine;

namespace ReflexCLI.DefaultParameterProcessors
{
	using Parameters;
	
	[ParameterProcessor(typeof(Vector2))]
	[ParameterProcessor(typeof(Vector3))]
	public class UnityVectorProcessor : ParameterProcessor
	{
		private static string[] TrimAndSplit(string inString)
		{
			char[] trimmers = new char[]{',', ' '};
			char[] separators = new char[]{','};

			return inString.Trim(trimmers).Split(separators);
		}

		public override object ConvertString(Type type, string inString)
		{
			var ret = GetMatchingNamedValue(type, inString);
			if(ret!=null)
				return ret;

			if(type == typeof(Vector2))
				return GetVec2(inString);

			if(type == typeof(Vector3))
				return GetVec3(inString);

			return null;
		}

		Vector3 GetVec3(string inString)
		{
			string[] components = TrimAndSplit(inString);
			int numComps = components.Length;
			return new Vector3(
					numComps > 0 ? float.Parse(components[0]) : 0.0f, 
					numComps > 1 ? float.Parse(components[1]) : 0.0f,
					numComps > 2 ? float.Parse(components[2]) : 0.0f);
		}

		Vector2 GetVec2(string inString)
		{
			return GetVec3(inString);
		}
	}
}

#endif //REFLEXCLI_ENABLED
