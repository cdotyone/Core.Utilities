using System;
using System.IO;
using System.Web;
using Civic.Core.Configuration;

namespace Civic.Core.Framework.Web.Modules
{
    /// <summary>
    /// This class is designed to help with developing with SPA type applications when developing and working on the intregration of the SPA HTML to the back-end web-api server.
    /// </summary>
    public class DevAppRewrite : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.MapRequestHandler += context_MapRequestHandler;
        }

        private void context_MapRequestHandler(object sender, EventArgs e)
        {
            var context = sender as HttpApplication;
            if (context != null)
            {
                var appname = ConfigurationFactory.ReadConfigValue<string>("appurlroot","");
                var path = context.Request.Url.AbsolutePath.ToLowerInvariant();

                if (!string.IsNullOrEmpty(appname))
                {
                    appname = appname.ToLowerInvariant();
                    if (!appname.StartsWith("/")) appname = "/" + appname;
                    if (!appname.EndsWith("/")) appname = appname + "/";
                }
                if (!string.IsNullOrEmpty(appname) && path.StartsWith(appname)) path = path.Substring(appname.Length);
                   
                // exclude things that we should not try to rewrite
                if (path == @"/" || path == "/default.aspx" || path.StartsWith("/api/") || path.StartsWith("/app/") || path == "/login.aspx" || path == "/logon.aspx")
                {
                    return;
                }

                if (!path.StartsWith("/")) path = "/" + path;
                // look to see if the file really exists in the app folder instead, if it does then rewrite it.
                path = (@"/app" + path);
                var filepath = context.Server.MapPath("~" + path);

                if (File.Exists(filepath))
                    context.Context.RewritePath(path);
            }
        }

        #endregion
    }
}
