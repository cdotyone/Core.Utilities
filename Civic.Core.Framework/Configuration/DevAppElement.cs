using Civic.Core.Configuration;

namespace Civic.Core.Framework.Configuration
{
    public class DevAppElement : NamedConfigurationElement
    {
        public DevAppElement(INamedElement config)
        {
            Attributes = config.Attributes;
            Children = config.Children;
        }

        public string DevUrl
        {
            get { return Attributes.ContainsKey("devurl") ? Attributes["devurl"] : null; }
            set { Attributes["devurl"] = value; }
        }

        public string DevRoot
        {
            get { return Attributes.ContainsKey("devroot") ? Attributes["devroot"] : null; }
            set { Attributes["devroot"] = value; }
        }

        public string ReloadPort
        {
            get { return Attributes.ContainsKey("reloadPort") ? Attributes["reloadPort"] : null; }
            set { Attributes["reloadPort"] = value; }
        }

        public string StripPaths
        {
            get { return Attributes.ContainsKey("stripPaths") ? Attributes["reloadPort"] : "bower_components,scripts,styles,fonts,images"; }
            set { Attributes["stripPaths"] = value; }
        }
    }
}
