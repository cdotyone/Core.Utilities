using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Civic.Core.Utility
{
	public static class ResourceHelper
	{

		public static string GetStringResource(Assembly assembly, string resourceName)
		{
			var resourceBuilder = new StringBuilder();
			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			{
				if (stream != null)
					using (var reader = new StreamReader(stream))
					{
						resourceBuilder.Append(reader.ReadToEnd());
					}
			}

			return resourceBuilder.ToString();
		}

		public static Byte[] GetBinaryResource(Assembly assembly, string resourceName)
		{
			using (Stream imageStream = assembly.GetManifestResourceStream(resourceName))
			{
				if (imageStream == null) return null;
				var imageByteArray = new Byte[imageStream.Length];
				imageStream.Read(imageByteArray, 0, (int) imageStream.Length);
				return imageByteArray;
			}
		}

	}
}
