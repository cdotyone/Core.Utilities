    using System;
using System.Configuration;
using System.IO;
using System.Web;
using Civic.Core.Caching;
using Civic.Core.Framework.Configuration;

namespace Civic.Core.Framework.Web
{
    public static class TemplateHelper
    {
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

        public static string GetPageTemplate(HttpRequest request, string appname, bool development, string cacheScope)
        {
            string cacheKey = appname.ToUpper() + "_TEMPLATEPAGE";

            var page = CacheManager.ReadCache(cacheScope, cacheKey, "");

            if (string.IsNullOrEmpty(page) || development)
            {

                var path1 = request.MapPath("~/" + appname + ".thtml");
                if (File.Exists(path1))
                {
                    page = File.ReadAllText(path1);
                }
                else
                {
                    if (development)
                    {
                        var path2 = path1.Replace(appname + ".thtml", "");
                        var siteConfig = DevAppProxySection.Current.Paths.Get(appname);
                        if (siteConfig != null)
                        {
                            var path3 = GetAbsolutePath(siteConfig.DevRoot + Path.DirectorySeparatorChar + "index.html", path2);
                            var path4 = GetAbsolutePath(siteConfig.DevRoot + Path.DirectorySeparatorChar + appname + ".html", path2);

                            if (File.Exists(path3)) path2 = path3;
                            if (File.Exists(path4)) path2 = path4;
                        }

                        if (siteConfig != null && File.Exists(path2))
                        {
                            page = File.ReadAllText(path2);

                            if (!string.IsNullOrEmpty(siteConfig.ReloadPort))
                            {
                                var autoreload =
                                    "<script type = \"text/javascript\">document.write('<script src=\"' + (location.protocol || 'http:') + '//' + (location.hostname || 'localhost') + ':" +
                                    siteConfig.ReloadPort +
                                    "/livereload.js?snipver=1\" type=\"text/javascript\"><\\/script>')</script>";
                                page = page.Replace("</body>", autoreload + "</body>");
                            }
                        }
                        else
                        {
                            throw new ConfigurationErrorsException(
                                string.Format("unable to locate {0}.(t)html, looked in {1} and {2}", appname, path1,
                                    path2));
                        }
                    }
                    else
                    {
                        path1 = request.MapPath("~/index.thtml");
                        if (File.Exists(path1))
                        {
                            page = File.ReadAllText(path1);
                        }
                        else
                        {

                            throw new ConfigurationErrorsException(
                                string.Format("unable to locate {0}.thtml, index.thtml, looked in {1}", appname,
                                    path1));
                        }
                    }
                }

                if (!development) CacheManager.WriteCache("PLL_TOYS", cacheKey, page);
            }

            return page;
        }
    }
}
