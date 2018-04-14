#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED
using System;
using System.Reflection;
using UnityEngine.Assertions;

namespace ReflexCLI
{
	public class MethodCommand : Command
	{
		private MethodInfo _Method = null;
		private MethodInfo Method {get {return _Method; } }

		public MethodCommand(MethodInfo method)
		{
			_Method = method;
		}
		
		public override MemberInfo Member {get {return _Method;} }

		public override bool IsStatic()
		{
			return _Method.IsStatic;
		}

		public override string[] GetCommandFormatAsTerms(string commandName)
		{
			var terms = new string[GetNumExpectedTerms()];
			int termIdx = 0;

			terms[termIdx++] = commandName;

			if(!IsStatic())
				terms[termIdx++] = InstanceString(_Method.DeclaringType);

			var parameters = _Method.GetParameters();
			for(int i=0, num=parameters.Length; i<num; i++)
			{
				var param = parameters[i];
				terms[termIdx] = ParameterString(param);
				if(param.IsOptional)
					terms[termIdx] += "=" + param.RawDefaultValue;

				termIdx++;
			}

			return terms;
		}

		public override int GetNumParams()
		{
			Assert.IsNotNull(_Method);

			int ret = _Method.GetParameters().Length;

			if(!IsStatic())
				ret ++;

			return ret;
		}

		public override Type GetParamType(int paramIdx)
		{
			Assert.IsNotNull(_Method);
			Assert.IsTrue(paramIdx >= 0 && paramIdx < GetNumParams() );

			if(!IsStatic())
				paramIdx--;

			if(paramIdx == -1)
				return _Method.DeclaringType;

			return _Method.GetParameters()[paramIdx].ParameterType;
		}

		public override object[] GetParameterAttributes(int paramIdx)
		{
			Assert.IsNotNull(_Method);
			Assert.IsTrue(paramIdx >= 0 && paramIdx < GetNumParams() );

			if(!IsStatic())
				paramIdx--;

			if(paramIdx >= 0)
				return _Method.GetParameters()[paramIdx].GetCustomAttributes(true);

			return null;
		}

		protected override object ExecuteCommandImpl(object instance, object[] parameters)
		{
			Assert.IsNotNull(_Method);
			return _Method.Invoke(instance, parameters);
		}

		protected override object[] ProcessParameters(string[] commandTerms)
		{
			Assert.IsNotNull(commandTerms);

			int termIdx = GetParamStartTermIdx();

			ParameterInfo[] parameterInfos = _Method.GetParameters();
			int numParams = parameterInfos.Length;
			var parameters = new object[numParams];

			for(int i=0; i<numParams; i++, termIdx++)
			{
				Type parameterType = parameterInfos[i].ParameterType;

				if(termIdx >= commandTerms.Length)
				{
					// No user param defined, but this shouldn't be necessary to handle explicitly
					// possibly once we hit the first optional param we can assume all are optional afterwards and resize the array?

					if(parameterInfos[i].IsOptional)
						parameters[i] = parameterInfos[i].RawDefaultValue;
					else
						parameters[i] = parameterType.IsValueType ? Activator.CreateInstance(parameterType) : null;
				}
				else
				{
					parameters[i] = ParameterProcessorRegistry.ConvertString(parameterType, commandTerms[termIdx]);
				}
			}

			return parameters;
		}
	}
}
#endif // REFLEXCLI_ENABLED
