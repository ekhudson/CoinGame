#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif



using System;
using System.Reflection;
using System.Collections.Generic;

namespace ReflexCLI.Parameters
{
	public static class NamedParameterRegistry
	{
#if REFLEXCLI_ENABLED
		const BindingFlags bindingFlags =	BindingFlags.Public | 
											BindingFlags.NonPublic | 
											BindingFlags.Static;

		private static Dictionary<Type, MemberInfo[]> NamedParamters = new Dictionary<Type, MemberInfo[]>();
#endif

		public static object GetMatchingNamedValue(Type type, string name)
		{
#if REFLEXCLI_ENABLED
			var values = GetNamedValues(type);

			for(int i=0, num=values.Length; i<num; i++)
			{
				if(values[i].Name == name)
				{
					var field = values[i] as FieldInfo;
					if(field != null)
						return field.GetValue(null);
					
					var property = values[i] as PropertyInfo;
					if(property != null)
						return property.GetGetMethod().Invoke(null, null);
				}
			}
#endif
			return null;
		}

		public static List<Suggestion> GetNamedValueSuggestions(Type type, string subStr)
		{
#if REFLEXCLI_ENABLED
			var ret = new List<Suggestion>();

			var values = GetNamedValues(type);
			for(int i=0, num=values.Length; i<num; i++)
			{
				if(Utils.StringStartsWith(values[i].Name, subStr))
				{
					var field = values[i] as FieldInfo;
					if(field != null)
					{
						ret.Add(field.Name);
						continue;
					}

					var property = values[i] as PropertyInfo;
					if(property != null)
						ret.Add(property.Name);
				}
			}

			return ret;
#else
			return null;
#endif   
		}

#if REFLEXCLI_ENABLED
		private static MemberInfo[] GetNamedValues(Type type)
		{
			MemberInfo[] members = null;

			if (NamedParamters.TryGetValue(type, out members))
				return members;

			var memberList = new List<MemberInfo>();

			var fieldInfos = type.GetFields(bindingFlags);
			for(int i=0, num=fieldInfos.Length; i<num; i++)
			{
				var field = fieldInfos[i];

				if(field.FieldType == type)
					memberList.Add(field);
			}

			var propertyInfos = type.GetProperties(bindingFlags);
			for(int i=0, num=propertyInfos.Length; i<num; i++)
			{
				var property = propertyInfos[i];

				if(property.ReflectedType == type && property.GetGetMethod() != null)
					memberList.Add(property);
			}

			members = memberList.ToArray();
			NamedParamters.Add(type, members);

			return members;
		}
#endif
	}
}