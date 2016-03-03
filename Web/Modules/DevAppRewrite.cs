using System;
using System.IO;
using System.Web;

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
                var appname = context.Request.ApplicationPath;
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

        public static String GetAbsolutePath(String relativePath, String basePath)
        {
            if (relativePath == null)
                return null;
            if (basePath == null)
                basePath = Path.GetFullPath("."); // quick way of getting current working directory
            else
                basePath = GetAbsolutePath(basePath, null); // to be REALLY sure ;)
                                                            // specific for windows paths starting on \ - they need the drive added to them.
                                                            // I constructed this piece like this for possible Mono support.
            if (!Path.IsPathRooted(relativePath) || "\\".Equals(Path.GetPathRoot(relativePath)))
            {
                if (relativePath.StartsWith(Path.DirectorySeparatorChar.ToString()))
                    return Path.GetFullPath(Path.Combine(Path.GetPathRoot(basePath), relativePath.TrimStart(Path.DirectorySeparatorChar)));
                return Path.GetFullPath(Path.Combine(basePath, relativePath));
            }
            return Path.GetFullPath(relativePath); // resolves any internal "..\" to get the true full path.
        }
    }
}
