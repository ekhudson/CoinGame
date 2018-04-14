using System;

namespace ReflexCLI.Attributes
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public class SubtypeOfAttribute : Attribute
	{
		private SubtypeOfAttribute() {}
		public SubtypeOfAttribute(Type baseType, bool allowBase = true, bool includeInterfaces = false)
		{
			BaseType = baseType;
			AllowBase = allowBase;
			IncludeInterfaces = includeInterfaces;
		}

		private Type BaseType = null;
		private bool AllowBase = true;
		private bool IncludeInterfaces = false;

		public bool IsValidType(Type type)
		{
			return	(AllowBase && type == BaseType) ||
					type.IsSubclassOf(BaseType) ||
					(IncludeInterfaces && type.IsInterface);
		}
	}
}