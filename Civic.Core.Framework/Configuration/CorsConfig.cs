using System.Linq;
using Civic.Core.Configuration;

namespace Civic.Core.Framework.Configuration
{
    public class CorsConfig : NamedConfigurationElement
    {
        #region Members

        private const string CORS_CONFIG_SECTION = "cors";
        private static CivicSection _coreConfig;
        private static CorsConfig _current;
        private bool _requireOptions;
        private int _maxAge;
        private bool _outputAllowCredentials;
        private string _allowedHeaders;
        private string _allowedMethods;

        #endregion

        public CorsConfig(INamedElement element)
        {
            if (element == null) element = new NamedConfigurationElement() { Name = SectionName };
            Children = element.Children;
            Attributes = element.Attributes;
            Name = element.Name;

            _requireOptions = Attributes.ContainsKey("requireOptions") && bool.Parse(Attributes["requireOptions"]);
            _maxAge = Attributes.ContainsKey("maxAge") ? int.Parse(Attributes["maxAge"]) : 1728000;
            _outputAllowCredentials = Attributes.ContainsKey("outputCredentialsFlag") && bool.Parse(Attributes["outputCredentialsFlag"]);
            _allowedHeaders = Attributes.ContainsKey("allowedHeaders") ? Attributes["allowedHeaders"] : "Content-Type, X-Requested-With, Authorization, *";
            _allowedMethods = Attributes.ContainsKey("allowedMethods") ? Attributes["allowedMethods"] : "GET, PUT, POST, HEAD, DELETE, OPTIONS";
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
                if (_current != null) return _current;

                if (_coreConfig == null) _coreConfig = CivicSection.Current;
                _current = new CorsConfig(_coreConfig.Children.ContainsKey(SectionName) ? _coreConfig.Children[SectionName] : null);
                return _current;
            }
        }

        /// <summary>
        /// True if the OPTIONS method is required before module executes
        /// </summary>
        public bool RequireOptions
        {
            get { return _requireOptions; }
            set { _requireOptions = value; Attributes["requireOptions"] = value.ToString(); }
        }

        /// <summary>
        /// the number of seconds before the client is required to request permission again, defaults to 1 day
        /// if it is set to zero or less it will not send the Access-Control-Max-Age header value
        /// </summary>
        public int MaxAge
        {
            get { return _maxAge; }
            set { _maxAge = value; Attributes["maxAge"] = value.ToString(); }
        }

        /// <summary>
        /// by default it does not require the client to have the credentials flag set
        /// </summary>
        public bool OutputAllowCredentials
        {
            get { return _outputAllowCredentials; }
            set { _outputAllowCredentials = value; Attributes["outputCredentialsFlag"] = value.ToString(); }
        }

        /// <summary>
        /// The headers that are allowed by the module, defaults to any,
        /// if this is null or blank than the Access-Control-Allow-Headers header will not be sent
        /// </summary>
        public string AllowedHeaders
        {
            get { return _allowedHeaders; }
            set { _allowedHeaders = value; Attributes["allowedHeaders"] = value; }
        }

        /// <summary>
        /// The list of allowed http methods, by default it is GET, PUT, POST, HEAD, DELETE, OPTIONS
        /// if blank or null than the Access-Control-Allow-Methods header will not be sent
        /// </summary>
        public string AllowedMethods
        {
            get { return _allowedMethods; }
            set { _allowedMethods = value; Attributes["allowedMethods"] = value; }
        }

        public bool Allowed(string domain)
		{
			domain = domain.ToLowerInvariant();
		    return Children.Values.Any(child => (!child.Attributes.ContainsKey("allow") || child.Attributes["allow"].ToLowerInvariant() == "true") && child.Name.ToLowerInvariant().EndsWith(domain));
		}
    }
}