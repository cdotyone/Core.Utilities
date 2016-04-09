using System.Collections.Generic;
using Civic.Core.Configuration;

namespace Civic.Core.Framework.Configuration
{
    public class DevlopmentAppConfig : NamedConfigurationElement
    {
        #region Members

        private const string DEVAPP_CONFIG_SECTION = "developmentApp";

        #endregion

        public DevlopmentAppConfig(INamedElement element)
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
            get { return DEVAPP_CONFIG_SECTION; }
        }

        /// <summary>
        /// To Return the Current devApp Proxy configuration
        /// </summary>
        public static DevlopmentAppConfig Current
        {
            get
            {
                if (_coreConfig == null) _coreConfig = CivicSection.Current;
                _current = new DevlopmentAppConfig(_coreConfig.Children.ContainsKey(SectionName) ? _coreConfig.Children[SectionName] : null);
                return _current;
            }
        }
        private static CivicSection _coreConfig;
        private static DevlopmentAppConfig _current;

        /// <summary>
        /// List of pages and roots
        /// </summary>
        public Dictionary<string,DevAppElement> Paths
        {
            get
            {
                var paths = new Dictionary<string, DevAppElement>();

                foreach (var element in Children)
                {
                    paths[element.Key] = new DevAppElement(element.Value);
                }

                return paths;
            }
        }
    }
}