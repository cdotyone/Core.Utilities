using System.Configuration;
using Civic.Core.Configuration;

namespace Civic.Core.Framework.Configuration
{
    public class DevAppProxySection : SerializableConfigurationSection
    {
        #region Members

        private const string DEVAPP_CONFIG_SECTION = "coreDevApp";
        private static DevAppProxySection _section;

        #endregion

        /// <summary>
        /// Property to return the Section Name 
        /// </summary>
        public static string SectionName
        {
            get { return DEVAPP_CONFIG_SECTION; }
        }

        /// <summary>
        /// To Return the Current coreCors Section
        /// </summary>
        public static DevAppProxySection Current
        {
            get
            {
				if (_section != null) return _section;

                _section = ConfigurationFactory.ReadConfigSection<DevAppProxySection>(DEVAPP_CONFIG_SECTION) ??
                           new DevAppProxySection();
	            
				return _section;
            }
        }

		/// <summary>
		/// True if IdentityHelp should translate the x-forwarded-for header to get client ip
		/// </summary>
		[ConfigurationProperty("devurl", IsRequired = false)]
		public string DevUrl
		{
            get { return (string)this["devurl"]; }
            set { this["devurl"] = value; }
		}

        [ConfigurationProperty("devroot", IsRequired = false, DefaultValue = false)]
        public string DevRoot
        {
            get { return (string)this["devroot"]; }
            set { this["devroot"] = value; }
        }
    }
}