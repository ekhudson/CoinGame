#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED
namespace ReflexCLI
{
	public class CommandException : System.ApplicationException
	{
		public CommandException(string message)
			: base(message)
		{ }

		public CommandException(string format, params object[] args)
			: base(string.Format(format, args))
		{ }
	}
}
#endif // REFLEXCLI_ENABLED
