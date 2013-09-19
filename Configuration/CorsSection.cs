using System.Collections.Generic;
using System.Configuration;
using Civic.Core.Configuration;

namespace Civic.Core.Framework.Configuration
{
    public class CorsSection : SerializableConfigurationSection
    {
        #region Members

        private const string CORS_CONFIG_SECTION = "coreCors";
		private List<string> _deny;
		private List<string> _allow;
		private static CorsSection _section;

        #endregion

        /// <summary>
        /// Property to return the Section Name 
        /// </summary>
        public static string SectionName
        {
            get { return CORS_CONFIG_SECTION; }
        }

        /// <summary>
        /// To Return the Current coreCors Section
        /// </summary>
		internal static CorsSection Current
        {
            get
            {
				if (_section != null) return _section;

				_section = ConfigurationFactory.ReadConfigSection<CorsSection>(CORS_CONFIG_SECTION) ??
					       ConfigurationFactory.ReadConfigSection<CorsSection>("EmbeddedResource", CORS_CONFIG_SECTION);
	            
				return _section;
            }
        }

		/// <summary>
		/// List of domains being configured for the CORS module
		/// </summary>
        [ConfigurationProperty("", IsRequired = false, IsDefaultCollection = true)]
		public NamedElementCollection<CorsDomainElement> Domains
        {
			get { return (NamedElementCollection<CorsDomainElement>)this[""]; }
            set { this[""] = value; }
        }

		/// <summary>
		/// True if the OPTIONS method is required before module executes
		/// </summary>
		[ConfigurationProperty("requireOptions", IsRequired = false, DefaultValue = false)]
		public bool RequireOptions
		{
			get { return (bool)this["requireOptions"]; }
			set { this["requireOptions"] = value; }
		}

		/// <summary>
		/// the number of seconds before the client is required to request permission again, defaults to 1 day
		/// if it is set to zero or less it will not send the Access-Control-Max-Age header value
		/// </summary>
		[ConfigurationProperty("maxAge", IsRequired = false, DefaultValue = 86400)]
		public int MaxAge
		{
			get { return (int)this["maxAge"]; }
			set { this["maxAge"] = value; }
		}

		/// <summary>
		/// by default it does not require the client to have the credentials flag set
		/// </summary>
		[ConfigurationProperty("outputCredentialsFlag", IsRequired = false, DefaultValue = false)]
		public bool OutputAllowCredentials
		{
			get { return (bool)this["outputCredentialsFlag"]; }
			set { this["outputCredentialsFlag"] = value; }
		}

		/// <summary>
		/// The headers that are allowed by the module, defaults to any,
		/// if this is null or blank than the Access-Control-Allow-Headers header will not be sent
		/// </summary>
		[ConfigurationProperty("allowedHeaders", IsRequired = false, DefaultValue = "Content-Type, *")]
		public string AllowedHeaders
		{
			get { return (string)this["allowedHeaders"]; }
			set { this["allowedHeaders"] = value; }
		}

		/// <summary>
		/// The list of allowed http methods, by default it is GET, PUT, POST, HEAD, DELETE, OPTIONS
		/// if blank or null than the Access-Control-Allow-Methods header will not be sent
		/// </summary>
		[ConfigurationProperty("allowedMethods", IsRequired = false, DefaultValue = "GET, PUT, POST, HEAD, DELETE, OPTIONS")]
		public string AllowedMethods
		{
			get { return (string)this["allowedMethods"]; }
			set { this["allowedMethods"] = value; }
		}

	    public IList<string> Allow
	    {
		    get
		    {
				if (_allow == null) Init();
			    return _allow;
		    }
	    }

		public IList<string> Deny
		{
			get
			{
				if(_deny==null) Init();
				return _deny;
			}
		}
		
		protected override void Init()
		{
			base.Init();

			_allow = new List<string>();
			_deny = new List<string>();

			foreach (var e in Domains)
			{
				if (e.Allow && !_allow.Contains(e.Name)) _allow.Add(e.Name.ToLowerInvariant());
				if (!e.Allow && !_allow.Contains(e.Name)) _deny.Add(e.Name.ToLowerInvariant());
			}
		}
    }
}