#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED
using System;
using System.Reflection;
using System.Collections.Generic;

namespace ReflexCLI
{
	public abstract class Command
	{
		public abstract MemberInfo Member {get; }
		public abstract bool IsStatic();
		public abstract int GetNumParams();
		public abstract Type GetParamType(int paramIdx);
		public abstract string[] GetCommandFormatAsTerms(string commandName);
		public virtual object[] GetParameterAttributes(int paramIdx) { return null; }

		protected abstract object[] ProcessParameters(string[] commandTerms);
		protected abstract object ExecuteCommandImpl(object instance, object[] parameters);

		public int GetNumExpectedTerms()
		{
			return GetNumParams() + 1;
		}

		protected int GetParamStartTermIdx()
		{
			return IsStatic() ? 1 : 2;
		}

		public object ExecuteCommand(string[] terms)
		{
			terms = Preprocess(terms);

			object instance = null;
			if(!IsStatic() && terms.Length > 1)
				instance = FindInstance(terms[1]);

			object[] parameters = ProcessParameters(terms);

			return ExecuteCommandImpl(instance, parameters);
		}

		private string[] Preprocess(string[] terms)
		{
			var numTerms = terms.Length;
			var ret = new List<string>(numTerms);
			for(int i=0; i<numTerms; i++)
			{
				if(string.IsNullOrEmpty(terms[i]) && i != numTerms-1)
					throw(new CommandException("received an empty string as one of the command terms. Only the final term can be empty"));

				ret.Add(terms[i]);
			}

			return ret.ToArray();
		}

		private object FindInstance(string instanceName)
		{
			if(IsStatic() || string.IsNullOrEmpty(instanceName))
				return null;

			return ParameterProcessorRegistry.ConvertString(Member.DeclaringType, instanceName);
		}

		protected static string NewValueString(Type type)
		{
			return ParameterString("NewValue", type);
		}
		
		protected static string InstanceString(Type type)
		{
			return ParameterString("Instance", type);
		}

		protected static string ParameterString(ParameterInfo parameterInfo)
		{
			return ParameterString(parameterInfo.Name, parameterInfo.ParameterType);
		}

		protected static string ParameterString(string parameterName, Type type)
		{
			return string.Format("{0}({1})", parameterName, type.Name);
		}

		public static implicit operator bool(Command command) {return command != null; }
	}
}
#endif // REFLEXCLI_ENABLED
