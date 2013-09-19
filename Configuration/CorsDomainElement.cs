using System.Configuration;
using Civic.Core.Configuration;

namespace Civic.Core.Framework.Configuration
{
    public class CorsDomainElement : NamedConfigurationElement
    {
		/// <summary>
		/// true if the domain is allowed, false to exclude domains and deny them
		/// </summary>
		[ConfigurationProperty("allow", IsRequired = false, DefaultValue = "true")]
		public bool Allow
		{
			get { return (bool)base["allow"]; }
			set { base["allow"] = value; }
		}
    }
}
