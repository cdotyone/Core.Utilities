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
                return HttpContext.Current.Request.UserHostAddress;
            }
        }  
    }
}
