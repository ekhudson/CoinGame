#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED
using System;
using System.Reflection;
using UnityEngine.Assertions;

namespace ReflexCLI
{
	public class PropertyCommand : Command
	{
		const bool INC_PRIVATE_ACCESSORS = true;

		private PropertyInfo _Property = null;
		public PropertyInfo Property {get { return _Property;} }

		private bool IsGettable {get {return _Property.GetGetMethod(INC_PRIVATE_ACCESSORS) != null;} }
		private bool IsSettable {get {return _Property.GetSetMethod(INC_PRIVATE_ACCESSORS) != null;} }
		private Type GetPropertyType()
		{
			Assert.IsNotNull(_Property);
			var getter = _Property.GetGetMethod(INC_PRIVATE_ACCESSORS);
			if(getter != null)
				return getter.ReturnType;
				
			var setter = _Property.GetSetMethod(INC_PRIVATE_ACCESSORS);
			if(setter != null)
			{
				var parameters = setter.GetParameters();
				if(parameters != null && parameters.Length > 0)
					return parameters[0].ParameterType;
			}
			Assert.IsFalse(true);
			return null;
		}
	
		public PropertyCommand(PropertyInfo property)
		{
			_Property = property;
		}

		public override MemberInfo Member {get {return _Property;} }

		public override bool IsStatic()
		{
			var accessors = _Property.GetAccessors(INC_PRIVATE_ACCESSORS);
			return accessors !=null && accessors.Length > 0 && accessors[0].IsStatic;
		}

		public override string[] GetCommandFormatAsTerms(string commandName)
		{
			// TODO - this can probably be fixed to be abstracted quite a bit.
			var terms = new string[GetNumExpectedTerms()];
			int termIdx = 0;

			terms[termIdx++] = commandName;

			if(!IsStatic())
				terms[termIdx++] = InstanceString(_Property.DeclaringType);

			if(IsSettable)
				terms[termIdx++] = NewValueString(GetPropertyType());

			return terms;
		}

		public override int GetNumParams()
		{
			return (IsStatic() ? 0 : 1) + (IsSettable ? 1 : 0);
		}

		public override Type GetParamType(int paramIdx)
		{
			Assert.IsNotNull(_Property);
			Assert.IsTrue(paramIdx >= 0 && paramIdx < GetNumParams() );

			if(!IsStatic() && paramIdx == 0)
				return _Property.DeclaringType;

			return GetPropertyType();
		}		

		protected override object ExecuteCommandImpl(object instance, object[] parameters)
		{
			Assert.IsNotNull(_Property);

			bool isSetting =    parameters != null && 
								parameters.Length > 0;

			if(isSetting)
			{
				var setter = _Property.GetSetMethod(INC_PRIVATE_ACCESSORS);
				if(setter != null)
					setter.Invoke(instance, parameters);
			}

			var getter = _Property.GetGetMethod(INC_PRIVATE_ACCESSORS);
			if(getter != null)
				return getter.Invoke(instance, null);

			return null;
		}

		protected override object[] ProcessParameters(string[] commandTerms)
		{
			Assert.IsNotNull(commandTerms);

			int paramIdx = GetParamStartTermIdx();
			if(commandTerms.Length > paramIdx)
				return new object[]{ParameterProcessorRegistry.ConvertString(GetPropertyType(), commandTerms[paramIdx]) };
			else
				return new object[0];
		}
	}
}
#endif // REFLEXCLI_ENABLED
