using System;

namespace ReflexCLI.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class ConsoleCommandClassCustomizerAttribute : Attribute
	{
		public ConsoleCommandClassCustomizerAttribute(string customName)
		{
			_customName = customName;
		}

		private string _customName = null;
		public string CustomName {get { return _customName;} }
	}
}