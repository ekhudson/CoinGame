using System;

namespace ReflexCLI.Parameters
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ParameterProcessorAttribute : Attribute
	{
		public ParameterProcessorAttribute(Type type)
		{
			_ProcessedType = type;
		}

		private Type _ProcessedType = null;
		public Type ProcessedType {get { return _ProcessedType;} }
	}
}