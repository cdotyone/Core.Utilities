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
        /// List of pages and roots
        /// </summary>
        [ConfigurationProperty("", IsRequired = false, IsDefaultCollection = true)]
        public NamedElementCollection<DevAppElement> Paths
        {
            get { return (NamedElementCollection<DevAppElement>)this[""]; }
            set { this[""] = value; }
        }
    }
}