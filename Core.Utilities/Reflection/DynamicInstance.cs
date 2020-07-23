using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Core.Utilities.Reflection
{
	internal static class DynamicInstance
	{
		private const string BAD_ARGUMENT = @"fullTypeName must contain a full type name including assembly.  received {0}";
		private static Dictionary<string, Assembly> _assemblyCache = new Dictionary<string, Assembly>();

		public static TI CreateInstance<TI>(string assemblyName, string typeName, params object[] parameters) where TI : class
		{
			return CreateInstance(assemblyName, typeName, parameters) as TI;
		}

		public static TI CreateInstance<TI>(string fullTypeName, params object[] parameters) where TI : class
		{
			if (!fullTypeName.Contains(",")) throw new Exception(string.Format(BAD_ARGUMENT, fullTypeName));
			var typeName = fullTypeName.Substring(0 ,fullTypeName.IndexOf(","));
			var assemblyName = fullTypeName.Substring(fullTypeName.IndexOf(",") + 1);

			return CreateInstance(assemblyName, typeName, parameters) as TI;
		}

		public static object CreateInstance(string fullTypeName, params object[] parameters)
		{
			if (!fullTypeName.Contains(",")) throw new Exception(string.Format(BAD_ARGUMENT, fullTypeName));
			var typeName = fullTypeName.Substring(0, fullTypeName.IndexOf(","));
			var assemblyName = fullTypeName.Substring(fullTypeName.IndexOf(",") + 1);

			return CreateInstance(assemblyName, typeName, parameters);
		}
		
		public static Assembly LoadAssembly(string assemblyname)
		{
			var key = assemblyname.ToUpper();
			if (_assemblyCache.ContainsKey(key)) return _assemblyCache[key];

			Assembly returnAssembly = null;
			try
			{
				returnAssembly = Assembly.Load(assemblyname);

				lock(_assemblyCache)
				{
					if(!_assemblyCache.ContainsKey(key))
					{
						_assemblyCache.Add(key, returnAssembly);
					}
				}
			}
			catch (ArgumentNullException) { }
			catch (FileNotFoundException) { }
			catch (BadImageFormatException) { }
			catch (FileLoadException) { }

			return returnAssembly;
		}

		public static Assembly LoadAssembly(AssemblyName assemblyname)
		{
			var key = assemblyname.FullName.ToUpper();
			if (_assemblyCache.ContainsKey(key)) return _assemblyCache[key];

			Assembly returnAssembly = null;
			try
			{
				returnAssembly = Assembly.Load(assemblyname);

				lock(_assemblyCache)
				{
					if(!_assemblyCache.ContainsKey(key))
					{
						_assemblyCache.Add(key, returnAssembly);
					}
				}
			}
			catch (ArgumentNullException) { }
			catch (FileNotFoundException) { }
			catch (BadImageFormatException) { }
			catch (FileLoadException) { }

			return returnAssembly;
		}

		public static Type GetType(string assemblyName, string typeName)
		{
			var type = Type.GetType(string.IsNullOrEmpty(assemblyName) ? typeName  : string.Format("{0}, {1}", typeName, assemblyName), false);

			if (type == null)
			{
				var asm = LoadAssembly(assemblyName);
				type = asm.GetType(typeName);
			}

			return type;
		}

		public static object CreateInstance(string assemblyName, string typeName, params object[] parameters)
		{
			var type = Type.GetType(string.IsNullOrEmpty(assemblyName) ? typeName : string.Format("{0}, {1}", typeName, assemblyName), false);
			
			if (type != null) return Activator.CreateInstance(type, parameters);

			return null;
		}
	}
}
