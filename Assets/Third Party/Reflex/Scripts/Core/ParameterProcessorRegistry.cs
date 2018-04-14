#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED

using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ReflexCLI
{
	using Parameters;
	using DefaultParameterProcessors;
	using Profiling;

	public static class ParameterProcessorRegistry
	{
		private static Dictionary<Type, ParameterProcessor> Processors = new Dictionary<Type, ParameterProcessor>();

		private static EnumProcessor EnumProcessor = new EnumProcessor();
		private static ParameterProcessor DefaultProcessor = new ParameterProcessor();
		private static UnityComponentProcessor ComponentProcessor = new UnityComponentProcessor();

		[RuntimeInitializeOnLoadMethod]
		private static void Init(){}

		static ParameterProcessorRegistry()
		{
			Assembly assembly = typeof(CommandRegistry).Assembly;
			Assert.IsNotNull(assembly);

			var types = assembly.GetTypes();
			for(int i=0, numI=types.Length; i<numI; i++)
			{
				var type = types[i];
				if(type.IsSubclassOf(typeof(ParameterProcessor)))
				{
					var processor = (ParameterProcessor)Activator.CreateInstance(type);
					if(processor != null)
					{
						var processedTypes = GetProcessedTypes(type);
						for(int t=0, numT=processedTypes.Count; t<numT; t++)
							Processors.Add(processedTypes[t], processor);
					}
				}
			}
		}

		private static List<Type> GetProcessedTypes(Type type)
		{
			List<Type> ret = new List<Type>();
			String className = type.Name;
			object[] classAttributes = type.GetCustomAttributes(typeof(ParameterProcessorAttribute), false);
			for(int i=0, num=classAttributes.Length; i<num; i++)
			{
				ParameterProcessorAttribute classAttribute = classAttributes[i] as ParameterProcessorAttribute;
				if(classAttribute != null)
					ret.Add(classAttribute.ProcessedType);
			}
			return ret;
		}

		private static ParameterProcessor GetProcessorFor(Type type)
		{
			Assert.IsNotNull(type);

			ParameterProcessor ret = null;
			if(!Processors.TryGetValue(type, out ret))
			{
				int subClassDepth = int.MaxValue;
				foreach(var typeProcPair in Processors)
				{
					Type handledType = typeProcPair.Key;
					ParameterProcessor processor = typeProcPair.Value;

					if(type.IsSubclassOf(handledType))
					{
						Type baseType = type;
						for(int i=0; i<subClassDepth; i++, baseType = baseType.BaseType)
						{
							if(baseType == handledType)
							{
								subClassDepth = i;
								ret = processor;
								break;
							}
						}
					}
				}
			}

			if(!ret)
			{
				// special cases
				if(type.IsEnum)
					ret = EnumProcessor;
				else if(type.IsInterface)
					ret = ComponentProcessor;
				else
					ret = DefaultProcessor;
			}
			
			Assert.IsNotNull(ret);
			return ret;
		}

		public static List<Suggestion> FindSuggestionsFor(Type type, object[] parameterAttributes, string subStr, int maxResults)
		{
			using(var p = new ScopedProfiler("ParameterProcessorRegistry.FindSuggestionsFor"))
				return GetProcessorFor(type).GetSuggestions(type, subStr, parameterAttributes, maxResults);
		}
		
		public static object ConvertString(Type type, string inString)
		{
			using(var p = new ScopedProfiler("ParameterProcessorRegistry.FindSuggestionsFor"))
				return GetProcessorFor(type).ConvertString(type, inString);
		}
	}
}
#endif // REFLEXCLI_ENABLED
