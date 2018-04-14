#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED

//#define ENABLE_PROFILING

using System;
using UnityEngine.Profiling;

namespace ReflexCLI.Profiling
{
	public struct ScopedProfiler : IDisposable
	{
		public ScopedProfiler(string name)
		{
#if ENABLE_PROFILING
			Profiler.BeginSample(name);
#endif
		}

		public ScopedProfiler(string name, UnityEngine.Object targetObject)
		{
#if ENABLE_PROFILING
			Profiler.BeginSample(name, targetObject);
#endif
		}

		void IDisposable.Dispose()
		{
#if ENABLE_PROFILING
				Profiler.EndSample();
#endif
		}
	}
}
#endif // REFLEXCLI_ENABLED
