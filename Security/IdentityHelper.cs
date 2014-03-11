using System.Web;

namespace Civic.Core.Framework.Security
{
    public static class IdentityManager
    {
        public static string Username
        {
            get
            {
                return "Unknown";
            }
        }

        public static string ClientMachine
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.UserHostName))
                    return HttpContext.Current.Request.UserHostName;
             
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.UserHostAddress))
                    return HttpContext.Current.Request.UserHostAddress;

                // TODO: THIS NEEDS TO BE OPTIONALLY TURNED ON
                foreach (var key in HttpContext.Current.Request.Headers.AllKeys)
                {
                    if (key.ToLowerInvariant() == "x-forwarded-for")
                    {
                        return HttpContext.Current.Request.Headers[key];
                    }
                }
                return "Unknown";
            }
        }  
    }
}
