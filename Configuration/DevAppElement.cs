using System.Configuration;
using Civic.Core.Configuration;

namespace Civic.Core.Framework.Configuration
{
    public class DevAppElement : NamedConfigurationElement
    {
        /// <summary>
        /// True if IdentityHelp should translate the x-forwarded-for header to get client ip
        /// </summary>
        [ConfigurationProperty("devurl", IsRequired = false)]
        public string DevUrl
        {
            get { return (string)this["devurl"]; }
            set { this["devurl"] = value; }
        }

        [ConfigurationProperty("devroot", IsRequired = false)]
        public string DevRoot
        {
            get { return (string)this["devroot"]; }
            set { this["devroot"] = value; }
        }

        [ConfigurationProperty("reloadPort", IsRequired = false)]
        public string ReloadPort
        {
            get { return (string)this["reloadPort"]; }
            set { this["reloadPort"] = value; }
        }

        [ConfigurationProperty("stripPaths", IsRequired = false, DefaultValue = "bower_components,scripts,styles,fonts,images")]
        public string StripPaths
        {
            get { return (string)this["stripPaths"]; }
            set { this["stripPaths"] = value; }
        }
    }
}
