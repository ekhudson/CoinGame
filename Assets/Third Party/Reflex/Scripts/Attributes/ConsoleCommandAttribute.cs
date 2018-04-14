using System;

namespace ReflexCLI.Attributes
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property)]
	public class ConsoleCommandAttribute : Attribute
	{
		public ConsoleCommandAttribute(string customName="")
		{
			_customName = customName;
		}

		private string _customName = null;
		public string CustomName {get { return _customName;} }
	}
}