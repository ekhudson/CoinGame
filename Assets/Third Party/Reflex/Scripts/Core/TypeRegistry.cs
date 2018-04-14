#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace ReflexCLI
{
	using Profiling;

	public static class TypeRegistry
	{
		private class TypeNameComparer : IComparer<Type>
		{
			int IComparer<Type>.Compare( Type x, Type y ) { return(StrComp.Compare(x.FullName, y.FullName)); }
		}

		private static CaseInsensitiveComparer StrComp = new CaseInsensitiveComparer();
		private static TypeNameComparer TypeComparer = new TypeNameComparer();

		private static List<Type> _AllTypes = null;
		public static List<Type> AllTypes
		{
			get
			{
				if(_AllTypes == null)
					DiscoverTypes(out _AllTypes) ;
				return _AllTypes;
			}
		}
		
		private static void DiscoverTypes(out List<Type> typesList, params string[] nameFilters)
		{
			using(var p = new ScopedProfiler("TypeRegistry.DiscoverTypes"))
			{
				typesList = new List<Type>();

				var assemblies = AppDomain.CurrentDomain.GetAssemblies();

				for(int a=0, numA=assemblies.Length; a<numA; a++)
				{
					if(nameFilters != null && nameFilters.Length > 0)
					{
						for(int f=0, numF=nameFilters.Length; f<numF; f++)
						{
							if(assemblies[a].FullName.StartsWith(nameFilters[f]))
								AddTypesFrom(assemblies[a], ref typesList);
						}
					}
					else
					{
						AddTypesFrom(assemblies[a], ref typesList);
					}
				}

				using(var p2 = new ScopedProfiler("SortTypes"))
					_AllTypes.Sort(TypeComparer);
			}
		}

		private static void AddTypesFrom(Assembly assembly, ref List<Type> typesList)
		{
			using(var p = new ScopedProfiler("TypeRegistry.AddTypesFrom"))
			{
				Type[] types;
				using(var p2 = new ScopedProfiler("GetExportedTypes()"))
					types = assembly.GetExportedTypes();

				for(int i=0, num=types.Length; i<num; i++)
				{
					var type = types[i];
					bool isStatic = type.IsAbstract && type.IsSealed;

					if(!isStatic && type.IsPublic)
						typesList.Add(type);
				}
			}
		}
	}
}
#endif // REFLEXCLI_ENABLED
