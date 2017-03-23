using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Civic.Core.Framework.Web.Handlers
{
    public class AngularLinkHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var uri = context.Request.Url;

            var angularPath = uri.PathAndQuery;
            angularPath = angularPath.Substring(context.Request.ApplicationPath.Length);

            var host = uri.AbsoluteUri.Replace(uri.PathAndQuery, "");
            if (context.Request.ApplicationPath != "/" && context.Request.ApplicationPath != "")
                host += context.Request.ApplicationPath;

            context.Response.Redirect(host + "/#" + angularPath);
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
