#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED
using System.Collections.Generic;

namespace ReflexCLI
{
	using Attributes;

	[ConsoleCommandClassCustomizer("Command")]
	public struct CommandDef
	{
		private string _Name;
		public string Name {get { return _Name;} }

		private Command _Command;
		public Command Command {get {return _Command; } }

		CommandDef(string name, Command command)
		{
			_Name = name;
			_Command = command;
		}

		public static implicit operator CommandDef(KeyValuePair<string, Command> pair)
		{
			return new CommandDef(pair.Key, pair.Value);
		}

		public string GetCommandFormat()
		{
			string ret = "";

			var terms = GetCommandFormatAsTerms();
			for(int i=0, num=terms.Length; i<num; i++)
				ret += terms[i] + " ";

			return ret.Trim(new char[] {' '});
		}

		public string[] GetCommandFormatAsTerms()
		{
			return Command.GetCommandFormatAsTerms(Name);
		}

		public bool IsValid()
		{
			return  Name != null &&
					Command != null;

					// TODO - Command.IsValid
		}

		[ConsoleCommand]
		public string Help()
		{
			return string.Format("{0}\n\tDeclaring Class: {1}\n\tMember Name: {2}", Name, Command.Member.DeclaringType.FullName, Command.Member.Name);
		}
	}
}

#endif // REFLEXCLI_ENABLED
