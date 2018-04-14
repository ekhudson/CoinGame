#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED
using System;
using System.Reflection;

using UnityEngine.Assertions;

namespace ReflexCLI
{
	public class FieldCommand : Command
	{
		private FieldInfo _Field = null;
		private FieldInfo Field {get {return _Field; } }

		public FieldCommand(FieldInfo field)
		{
			_Field = field;
		}

		public override MemberInfo Member {get {return _Field;} }

		public override bool IsStatic() 
		{
			return _Field.IsStatic;
		}

		public bool IsReadOnly()
		{
			return _Field.IsLiteral || _Field.IsInitOnly;
		}

		public override string[] GetCommandFormatAsTerms(string commandName)
		{
			var terms = new string[GetNumExpectedTerms()];
			int termIdx = 0;

			terms[termIdx++] = commandName;

			if(!IsStatic())
				terms[termIdx++] = InstanceString(_Field.DeclaringType);

			if(!IsReadOnly())
			terms[termIdx++] = NewValueString(_Field.FieldType);

			return terms;
		}

		public override int GetNumParams()
		{
			return (IsStatic() ? 0 : 1) + (IsReadOnly() ? 0 : 1);
		}

		public override Type GetParamType(int paramIdx)
		{
			Assert.IsNotNull(_Field);
			Assert.IsTrue(paramIdx >= 0 && paramIdx < GetNumParams() );

			if(!IsStatic() && paramIdx == 0)
				return _Field.DeclaringType;

			return _Field.FieldType;
		}

		protected override object ExecuteCommandImpl(object instance, object[] parameters)
		{
			if(parameters != null && parameters.Length > 0 && !IsReadOnly())
				_Field.SetValue(instance, parameters[0]);

			return _Field.GetValue(instance);
		}

		protected override object[] ProcessParameters(string[] commandTerms)
		{
			Assert.IsNotNull(commandTerms);

			int paramIdx = GetParamStartTermIdx();
			if(commandTerms.Length > paramIdx)
				return new object[]{ParameterProcessorRegistry.ConvertString(_Field.FieldType, commandTerms[paramIdx])};
			else
				return new object[0];
		}
	}
}
#endif // REFLEXCLI_ENABLED
