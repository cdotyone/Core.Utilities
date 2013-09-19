using System;
using System.Reflection;
using Civic.Core.Configuration;

namespace Civic.Core.Framework
{
    public static class ReflectionHelper
    {
        public static object GetMethodValue(string assemblyName, string assemblyTypeName, string method, Object[] args)
        {
            object returnObject = null;
            try
            {
				Type assemblyType = DynamicInstance.GetType(assemblyName, assemblyTypeName);
                if (assemblyType != null)
                {
                    returnObject = assemblyType.InvokeMember(method, BindingFlags.InvokeMethod, null, null, args);
                }
            }
            catch (ArgumentNullException) { }
            catch (ArgumentException) { }
            catch (MethodAccessException) { }
            catch (MissingFieldException) { }
            catch (MissingMethodException) { }
            catch (TargetException) { }
            catch (AmbiguousMatchException) { }
            catch (InvalidOperationException) { }

            return returnObject;
        }

        public static object GetPropertyValue(string assemblyName, string assemblyTypeName, string name)
        {
            object returnValue = null;
            try
            {
				Type assemblyType = DynamicInstance.GetType(assemblyName, assemblyTypeName);
                if (assemblyType != null)
                    returnValue = assemblyType.GetProperty(name).GetValue(null, null);
            }
            catch (AmbiguousMatchException) { }
            catch (ArgumentNullException) { }

            return returnValue;

        }

		/// <summary>
		/// Returns the value of an Attribute if it has been applied to an enum value.
		/// </summary>
		/// <typeparam name="TAttribute">The type of the attribute to be retrieved. It must inherit from <see cref="Attribute"/></typeparam>
		/// <param name="value">The enum value that has the Attribute applied.</param>
		/// <returns>The Attribute applied to the enum value</returns>
		public static TAttribute GetAttributeFromEnum<TAttribute>(object value) where TAttribute : Attribute
		{
			Type pobjType = value.GetType();
			FieldInfo pobjFieldInfo = pobjType.GetField(Enum.GetName(pobjType, value));

			foreach (Object attribute in pobjFieldInfo.GetCustomAttributes(typeof(TAttribute), false))
			{
				if (attribute is TAttribute)
				{
					return ((TAttribute)attribute);
				}
			}
			return null;
		}
	}
}