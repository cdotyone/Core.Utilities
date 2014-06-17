using System.Configuration;
using Civic.Core.Configuration;

namespace Civic.Core.Framework.Configuration
{
    public class IdentitySection : SerializableConfigurationSection
    {
        #region Members

        private const string IDENTITY_CONFIG_SECTION = "coreIdentity";
        private static IdentitySection _section;

        #endregion

        /// <summary>
        /// Property to return the Section Name 
        /// </summary>
        public static string SectionName
        {
            get { return IDENTITY_CONFIG_SECTION; }
        }

        /// <summary>
        /// To Return the Current coreCors Section
        /// </summary>
        internal static IdentitySection Current
        {
            get
            {
				if (_section != null) return _section;

                _section = ConfigurationFactory.ReadConfigSection<IdentitySection>(IDENTITY_CONFIG_SECTION) ??
                           ConfigurationFactory.ReadConfigSection<IdentitySection>("EmbeddedResource", IDENTITY_CONFIG_SECTION);
	            
				return _section;
            }
        }

		/// <summary>
		/// True if IdentityHelp should translate the x-forwarded-for header to get client ip
		/// </summary>
		[ConfigurationProperty("xforward", IsRequired = false, DefaultValue = false)]
		public bool TransformXForwardedFor
		{
            get { return (bool)this["xforward"]; }
            set { this["xforward"] = value; }
		}
    }
}