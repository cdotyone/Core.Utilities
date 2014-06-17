using System.Web;
using Civic.Core.Framework.Configuration;

namespace Civic.Core.Framework.Security
{
    public static class IdentityManager
    {
        // ReSharper disable ConditionIsAlwaysTrueOrFalse
        public static string Username
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.User==null || HttpContext.Current.User.Identity==null || !HttpContext.Current.User.Identity.IsAuthenticated) return "Unknown";
                return HttpContext.Current.User.Identity.Name;
            }
        }
        // ReSharper restore ConditionIsAlwaysTrueOrFalse

        public static string ClientMachine
        {
            get
            {
                var client = "";
                if (HttpContext.Current == null) client = System.Environment.MachineName;
                else 
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.UserHostName))
                    client = HttpContext.Current.Request.UserHostName;
                else if (!string.IsNullOrEmpty(HttpContext.Current.Request.UserHostAddress))
                    client = HttpContext.Current.Request.UserHostAddress;

                if (HttpContext.Current != null && IdentitySection.Current.TransformXForwardedFor)
                {
                    // x-forwarded-for check
                    foreach (var key in HttpContext.Current.Request.Headers.AllKeys)
                    {
                        if (key.ToLowerInvariant() == "x-forwarded-for")
                        {
                            client = HttpContext.Current.Request.Headers[key];
                            break;
                        }
                    }
                }

                if (client == "::1") client = System.Environment.MachineName;
                if (string.IsNullOrEmpty(client)) client = "Unknown";
                return client;
            }
        }  
    }
}
