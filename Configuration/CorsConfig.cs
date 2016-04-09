using System.Configuration;
using System.Linq;
using Civic.Core.Configuration;

namespace Civic.Core.Framework.Configuration
{
    public class CorsConfig : NamedConfigurationElement
    {
        #region Members

        private const string CORS_CONFIG_SECTION = "cors";

        #endregion

        public CorsConfig(INamedElement element)
        {
            if (element == null) element = new NamedConfigurationElement() { Name = SectionName };
            Children = element.Children;
            Attributes = element.Attributes;
            Name = element.Name;
        }

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
		internal static CorsConfig Current
        {
            get
            {
                if (_coreConfig == null) _coreConfig = CivicSection.Current;
                _current = new CorsConfig(_coreConfig.Children.ContainsKey(SectionName) ? _coreConfig.Children[SectionName] : null);
                return _current;
            }
        }
        private static CivicSection _coreConfig;
        private static CorsConfig _current;

		/// <summary>
		/// True if the OPTIONS method is required before module executes
		/// </summary>
		[ConfigurationProperty("requireOptions", IsRequired = false, DefaultValue = false)]
		public bool RequireOptions
		{
            get { return Attributes.ContainsKey("requireOptions") && bool.Parse(Attributes["requireOptions"]); }
            set { Attributes["requireOptions"] = value.ToString(); }
		}

		/// <summary>
		/// the number of seconds before the client is required to request permission again, defaults to 1 day
		/// if it is set to zero or less it will not send the Access-Control-Max-Age header value
		/// </summary>
        [ConfigurationProperty("maxAge", IsRequired = false, DefaultValue = 1728000)]
		public int MaxAge
		{
            get { return Attributes.ContainsKey("maxAge") ? int.Parse(Attributes["maxAge"]) : 1728000; }
            set { Attributes["maxAge"] = value.ToString(); }
		}

		/// <summary>
		/// by default it does not require the client to have the credentials flag set
		/// </summary>
		public bool OutputAllowCredentials
		{
            get { return Attributes.ContainsKey("outputCredentialsFlag") && bool.Parse(Attributes["outputCredentialsFlag"]); }
            set { Attributes["outputCredentialsFlag"] = value.ToString(); }
		}

		/// <summary>
		/// The headers that are allowed by the module, defaults to any,
		/// if this is null or blank than the Access-Control-Allow-Headers header will not be sent
		/// </summary>
		public string AllowedHeaders
		{
            get { return Attributes.ContainsKey("allowedHeaders") ? Attributes["allowedHeaders"] : "Content-Type, X-Requested-With, Authorization, *"; }
			set { Attributes["allowedHeaders"] = value; }
		}

		/// <summary>
		/// The list of allowed http methods, by default it is GET, PUT, POST, HEAD, DELETE, OPTIONS
		/// if blank or null than the Access-Control-Allow-Methods header will not be sent
		/// </summary>
		public string AllowedMethods
		{
            get { return Attributes.ContainsKey("allowedMethods") ? Attributes["allowedMethods"] : "GET, PUT, POST, HEAD, DELETE, OPTIONS"; }
			set { Attributes["allowedMethods"] = value; }
		}

		public bool Allowed(string domain)
		{
			domain = domain.ToLowerInvariant();
		    return Children.Values.Any(child => (!child.Attributes.ContainsKey("allow") || child.Attributes["allow"].ToLowerInvariant() == "true") && child.Name.ToLowerInvariant().EndsWith(domain));
		}
    }
}