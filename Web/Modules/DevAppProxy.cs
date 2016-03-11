using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Civic.Core.Framework.Configuration;

namespace Civic.Core.Framework.Web.Modules
{
    /// <summary>
    /// This class is designed to help with developing with SPA type applications when developing and working on the intregration of the SPA HTML to the back-end web-api server.
    /// </summary>
    public class DevAppProxy : IHttpModule
    {
        private static Mutex _mutex = new Mutex();
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += request;
        }

        private void request(object sender, EventArgs e)
        {
            var config = DevAppProxySection.Current;
            var context = sender as HttpApplication;
            if (context != null)
            {
                if (context.Request.ApplicationPath != null)
                {
                    var appname = context.Request.ApplicationPath.ToLowerInvariant();
                    var path = context.Request.Url.AbsolutePath.ToLowerInvariant();

                    if (!string.IsNullOrEmpty(appname))
                    {
                        appname = appname.ToLowerInvariant();
                        if (!appname.StartsWith("/")) appname = "/" + appname;
                        if (!appname.EndsWith("/")) appname = appname + "/";
                    }
                    if (!string.IsNullOrEmpty(appname) && path.StartsWith(appname)) path = path.Substring(appname.Length);

                    // exclude things that we should not try to rewrite
                    if (string.IsNullOrEmpty(path) || path.EndsWith(@"/") || path.EndsWith(".aspx") || path.StartsWith("api/"))
                    {
                        return;
                    }

                    if (path.StartsWith("/")) path = path.Substring(0);
                    var dirParts = new List<string>(path.Split('/'));

                    var angularName = dirParts[0];
                    var projectConfig = config.Paths.Get(angularName);

                    var stripDirList = new List<string>(projectConfig.StripPaths.Split(','));
                    stripDirList.Insert(0, angularName);
                    foreach (var dirname in stripDirList)
                    {
                        if (path.StartsWith(angularName + "/" + dirname, StringComparison.CurrentCultureIgnoreCase))
                        {
                            dirParts.RemoveAt(0);
                            path = string.Join("/", dirParts);
                            break;
                        }
                    }

                    var root = GetAbsolutePath(projectConfig.DevRoot, context.Server.MapPath("~/"));

                    var filePath = root + Path.DirectorySeparatorChar + path.Replace('/',Path.DirectorySeparatorChar);

                    var cnt = 5;
                    while (!File.Exists(filePath) && !File.Exists(filePath.Replace(@"\app\",@"\.tmp\")) &&  filePath != root && cnt>0)
                    {
                        if (path.StartsWith("bower_components", StringComparison.CurrentCultureIgnoreCase)) break;
                        path = "/" + string.Join("/", dirParts);

                        filePath = root + path.Replace('/', Path.DirectorySeparatorChar);
                        cnt--;
                    }
                    if (!File.Exists(filePath) && !File.Exists(filePath.Replace(@"\app\", @"\.tmp\")) && !path.StartsWith("bower_components")) return;
                    if (!path.StartsWith("/")) path = "/" + path;


                    HttpClient client = new HttpClient();
                    try
                    {
                        var url = projectConfig.DevUrl.TrimEnd('/');
                    
                        Trace.TraceInformation("Request To:{0}", context.Request.Url);

                        var response = client.GetAsync(url + path).Result;
                        if (response != null)
                        {
                            using (MemoryStream memStream = new MemoryStream())
                            {
                                _mutex.WaitOne(15000);
                                Task task = response.Content.ReadAsStreamAsync().ContinueWith(t =>
                                {
                                    CopyStream(t.Result, memStream);
                                });

                                task.Wait();
                                _mutex.ReleaseMutex();

                                context.Response.Clear();
                                foreach (var header in response.Content.Headers)
                                {
                                    if (header.Value != null)
                                    {
                                        context.Response.AddHeader(header.Key, header.Value.FirstOrDefault());
                                    }
                                }

                                context.Response.BinaryWrite(memStream.GetBuffer());
                                context.Response.End();
                            }
                        }
                    }
                    catch (ThreadAbortException)
                    {
                        // do nothing
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceInformation("Exception {0}", ex.Message);
                    }
                }
            }
        }

        public static void CopyStream(Stream input, Stream output)
        {
            var buffer = new byte[5012];//4096
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

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
                else
                    return Path.GetFullPath(Path.Combine(basePath, relativePath));
            }
            else
                return Path.GetFullPath(relativePath); // resolves any internal "..\" to get the true full path.
        }
        #endregion
    }




}
