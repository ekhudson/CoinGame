#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

using System;
using System.Collections.Generic;

namespace ReflexCLI.Parameters
{
	public class ParameterProcessor
	{
		public virtual List<Suggestion> GetSuggestions(Type type, string subStr, object[] attributes, int maxResults)
		{
#if REFLEXCLI_ENABLED
			return GetNamedValueSuggestions(type, subStr);
#else
			return null;
#endif
		}

		public virtual object ConvertString(Type type, string inString)
		{
#if REFLEXCLI_ENABLED
			object ret = GetMatchingNamedValue(type, inString);
			return ret != null ? ret : Convert.ChangeType(inString, type);
#else
			return null;
#endif
		}

		protected List<Suggestion> GetMatchingSuggestions(List<Suggestion> allSuggestions, string subStr, int maxResults)
		{
			List<Suggestion> ret = null;
#if REFLEXCLI_ENABLED
			if(String.IsNullOrEmpty(subStr))
				return allSuggestions;

			ret = new List<Suggestion>();

			maxResults = Math.Min(allSuggestions.Count, maxResults);
			for(int i=0; i<maxResults; i++)
			{
				if(StringStartsWith(allSuggestions[i].Value, subStr))
					ret.Add(allSuggestions[i]);
			}
#endif
			return ret;
		}

		protected List<Suggestion> GetMatchingSuggestions<T>(IEnumerable<T> allSuggestions, string subStr, int maxResults)
		{
			List<Suggestion> ret = null;
#if REFLEXCLI_ENABLED
			ret = new List<Suggestion>();
			foreach(var suggestion in allSuggestions)
			{
				var suggestionStr = suggestion.ToString();
				if(StringStartsWith(suggestionStr, subStr))
					ret.Add(suggestionStr);

				if(ret.Count >= maxResults)
					break;
			}
#endif
			return ret;
		}

		protected static bool StringStartsWith(string inString, string subStr)
		{
#if REFLEXCLI_ENABLED
			return Utils.StringStartsWith(inString, subStr);
#else
			return false;
#endif
		}

		protected static bool StringEquals(string inString, string candidate)
		{
#if REFLEXCLI_ENABLED
			return Utils.StringStartsWith(inString, candidate);
#else
			return false;
#endif
		}

		protected static object GetMatchingNamedValue(Type type, string name)
		{
			return NamedParameterRegistry.GetMatchingNamedValue(type, name);
		}

		protected static List<Suggestion> GetNamedValueSuggestions(Type type, string subStr)
		{
			return NamedParameterRegistry.GetNamedValueSuggestions(type, subStr);
		}

		protected static bool AddHierarchically(List<Suggestion> suggestions, string name, string subStr, char separator)
		{
#if REFLEXCLI_ENABLED
			if(!StringStartsWith(name, subStr))
				return false;
           
			if(!Settings.HierarchicalAutocomplete)
			{
				suggestions.Add(name);
				return true;
			}

			int levels = 0;
			for(int i=0, num=subStr.Length; i<num; i++)
				levels += subStr[i] == separator ? 1 : 0;

			for(int i=0; i<name.Length; i++)
			{
				levels -= name[i] == separator ? 1 : 0;
				if(levels < 0)
					name = name.Substring(0, i);
			}

			for(int i=0, num=suggestions.Count; i<num; i++)
			{
				if(suggestions[i].Value == name)
					return false;
			}

			if(levels < 0)
				suggestions.Add(new Suggestion(name, string.Format("{0}{1} ...", name, separator)));
			else
				suggestions.Add(name);
			return true;
#else
			return false;
#endif
		}

		public static implicit operator bool(ParameterProcessor p) {return p != null; }
	}
}