#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED

namespace ReflexCLI
{
	using Attributes;

	[ConsoleCommandClassCustomizer("Version")]
	public static class Version
	{
		[ConsoleCommand]
		public const int Major = 1;
		[ConsoleCommand]
		public const int Minor = 1;
		[ConsoleCommand]
		public const int Patch = 1;

		[ConsoleCommand("")]
		public static readonly string String = string.Format("{0}.{1}.{2}", Major, Minor, Patch);
	}
}

#endif // REFLEXCLI_ENABLED