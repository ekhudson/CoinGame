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
	using Attributes;

	public static class CommandRegistry
	{
		private class StringComparerIgnoreCase : IEqualityComparer<string>
		{
			public bool Equals(string x, string y)
			{
				if (x != null && y != null)
				{
					return x.ToLowerInvariant() == y.ToLowerInvariant();
				}
				return false;
			}

			public int GetHashCode(string obj)
			{
				return obj.ToLowerInvariant().GetHashCode();
			}
		}

		private static Dictionary<string, Command> Commands = null;

		[RuntimeInitializeOnLoadMethod]
		private static void Init(){}

		static CommandRegistry()
		{
			Rebuild();
		}

		public static void Rebuild()
		{
			const BindingFlags bindings =	BindingFlags.Public |
											BindingFlags.NonPublic | 
											BindingFlags.Static |
											BindingFlags.Instance;

			Commands = new Dictionary<string, Command>(new StringComparerIgnoreCase());

			Assembly assembly = typeof(CommandRegistry).Assembly;
			Assert.IsNotNull(assembly);

			var types = assembly.GetTypes();
			for(int i=0, num=types.Length; i<num; i++)
			{
				var type = types[i];
				string className = GetClassName(type);

				var memberInfos = type.GetMembers(bindings);
				for(int j=0, jNum=memberInfos.Length; j<jNum; j++)
				{
					var memberInfo = memberInfos[j];
					if(type != memberInfo.DeclaringType)
						continue;

					var attribute = GetCommandAttribute(memberInfo);
					if(attribute != null)
					{
						var wrapper = GetWrapperFor(memberInfo);
						if(wrapper)
							AddCommand(wrapper, attribute, className);
					}
				}
			}
		}

		private static ConsoleCommandAttribute GetCommandAttribute(MemberInfo memberInfo)
		{
			object[] attributes = memberInfo.GetCustomAttributes(typeof(ConsoleCommandAttribute), true);

			return attributes.Length > 0 ? attributes[0] as ConsoleCommandAttribute : null;
		}

		private static Command GetWrapperFor(MemberInfo memberInfo)
		{
			Assert.IsNotNull(memberInfo);

			Command ret = null;
			var memberType = memberInfo.MemberType;
			
			if(0 != (memberType & MemberTypes.Field))
				ret = new FieldCommand(memberInfo as FieldInfo);
			else if(0 != (memberType & MemberTypes.Method))
				ret = new MethodCommand(memberInfo as MethodInfo);
			else if(0 != (memberType & MemberTypes.Property))
				ret = new PropertyCommand(memberInfo as PropertyInfo);
			
			return (ret && ret.Member != null) ? ret : null;
		}

		private static void AddCommand(Command CommandWrapper, ConsoleCommandAttribute attribute, string prefix)
		{
			Assert.IsNotNull(CommandWrapper.Member);
			Assert.IsNotNull(attribute);
			Assert.IsNotNull(prefix);

			string methodName = GetCommandName(CommandWrapper.Member, attribute);

			Assert.IsFalse(String.IsNullOrEmpty(methodName));

			string commandString = prefix.Length > 0 ? prefix + '.' + methodName : methodName;
			Commands.Add(commandString, CommandWrapper);
		}

		private static string GetClassName(Type type)
		{
			object[] classAttributes = type.GetCustomAttributes(typeof(ConsoleCommandClassCustomizerAttribute), false);
			if(classAttributes.Length > 0)
			{
				ConsoleCommandClassCustomizerAttribute classAttribute = classAttributes[0] as ConsoleCommandClassCustomizerAttribute;
				if(classAttribute != null)
					return classAttribute.CustomName;
			}

			switch(Settings.DefaultNamingConvention)
			{
			case ENamingConvention.NoPrefix:	return "";
			case ENamingConvention.ClassPrefix:	return type.Name;
			case ENamingConvention.NamespaceAndClassPrefix:	return type.FullName;
			default:
				Assert.IsFalse(true, "Unrecognised Naming Convention specified");
				return type.FullName;
			}
		}

		private static string GetCommandName(MemberInfo memberInfo, ConsoleCommandAttribute attribute)
		{
			string customMethodName = attribute == null ? "" : attribute.CustomName;
			return String.IsNullOrEmpty(customMethodName) ? memberInfo.Name : customMethodName;
		}

		public static Dictionary<string, Command> GetCommands()
		{
			// shallow copy should be fine to prevent accidental messing with registry
			return new Dictionary<string, Command>(Commands, new StringComparerIgnoreCase());
		}

		public static CommandDef GetCommand(string commandString)
		{
			foreach(CommandDef commandDef in Commands)
			{
				if(commandDef.Name.Equals(commandString, StringComparison.InvariantCultureIgnoreCase))
					return commandDef;
			}
			return new CommandDef();
		}
	}
}

#endif // REFLEXCLI_ENABLED
