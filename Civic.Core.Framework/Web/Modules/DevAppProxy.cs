using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
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
            var config = DevlopmentAppConfig.Current;
            var context = sender as HttpApplication;
            if (context != null)
            {
                if (context.Request.ApplicationPath != null)
                {
                    // determine the application name and remove it from the request path name
                    var appname = context.Request.ApplicationPath.ToLowerInvariant();
                    var path = context.Request.Url.AbsolutePath;

                    if (!string.IsNullOrEmpty(appname))
                    {
                        appname = appname.ToLowerInvariant();
                        if (!appname.StartsWith("/")) appname = "/" + appname;
                        if (!appname.EndsWith("/")) appname = appname + "/";
                    }
                    if (!string.IsNullOrEmpty(appname) && path.StartsWith(appname,StringComparison.InvariantCultureIgnoreCase)) path = path.Substring(appname.Length);

                    // are we supposed to process this page
                    if (string.IsNullOrEmpty(path) || path.EndsWith(".aspx")) return;

                    // normallize path
                    path = path.TrimEnd('/');
                    var dirParts = new List<string>(path.Split('/'));

                    // determine if this request has an assiciated project configuration
                    var angularName = dirParts[0];
                    var projectConfig = config.Paths.ContainsKey(angularName) ? config.Paths[angularName] : null;

                    if (projectConfig == null)
                    {
                        angularName = config.Paths.Keys.FirstOrDefault();
                        projectConfig = config.Paths.ContainsKey(angularName) ? config.Paths[angularName] : null;
                    }

                    // exclude things that we should not try to rewrite, one last check to see if we need to process this request
                    if (dirParts.Count==1 || angularName=="api" || projectConfig == null)
                    {
                        return;
                    }

                    //// remove lead folders we don't need when request from dev grunt web server
                    //var stripDirList = new List<string>(projectConfig.StripPaths.Split(','));
                    //stripDirList.Insert(0, angularName);
                    //foreach (var dirname in stripDirList)
                    //{
                    //    if (path.StartsWith(angularName + "/" + dirname, StringComparison.InvariantCultureIgnoreCase))
                    //    {
                    //        dirParts.RemoveAt(0);
                    //        path = string.Join("/", dirParts);
                    //        break;
                    //    }
                    //}

                    var root = TemplateHelper.GetAbsolutePath(projectConfig.DevRoot, context.Server.MapPath("~/"));
                    var filePath = root + Path.DirectorySeparatorChar + path.Replace('/',Path.DirectorySeparatorChar);

                    // try to find the physical file on the file system, removing the lead directory until it is found, or we determine we can not find it
                    var cnt = 5;
                    while (!File.Exists(filePath) && !File.Exists(filePath.Replace(@"\app\", @"\.tmp\")) && filePath != root && cnt > 0 && dirParts.Count>0)
                    {
                        if (path.StartsWith("/")) path = path.Substring(1);
                        if (path.StartsWith("bower_components", StringComparison.CurrentCultureIgnoreCase)) break;
                        dirParts.RemoveAt(0);
                        path = "/" + string.Join("/", dirParts);

                        filePath = root + path.Replace('/', Path.DirectorySeparatorChar);
                        cnt--;
                    }

                    // see if we could find it, if not bounce out
                    if (!File.Exists(filePath) && !File.Exists(filePath.Replace(@"\app\", @"\.tmp\")) && !path.StartsWith("bower_components")) return;
                    if (!path.StartsWith("/")) path = "/" + path;


                    context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    context.Response.Cache.SetNoStore();

                    try
                    {
                        var done = false;
                        using (MemoryStream memStream = new MemoryStream())
                        {

                            if (File.Exists(filePath))
                            {
                                context.Response.WriteFile(filePath);
                                context.Response.ContentType = MimeMapping.GetMimeMapping(filePath);
                                done = true;
                            }
                            else
                            {
                                if (File.Exists(filePath.Replace(@"\app\", @"\.tmp\")))
                                {
                                    context.Response.WriteFile(filePath.Replace(@"\app\", @"\.tmp\"));
                                    context.Response.ContentType = MimeMapping.GetMimeMapping(filePath.Replace(@"\app\", @"\.tmp\"));

                                    done = true;
                                }
                                else
                                {
                                    if (File.Exists(filePath.Replace(@"\app\", @"\")))
                                    {
                                        context.Response.WriteFile(filePath.Replace(@"\app\", @"\"));
                                        context.Response.ContentType = MimeMapping.GetMimeMapping(filePath.Replace(@"\app\", @"\"));
                                        done = true;
                                    }
                                }
                            }

                            if (!done)
                            {
                                HttpClient client = new HttpClient();
                                var url = projectConfig.DevUrl.TrimEnd('/');

                                Trace.TraceInformation("Request To:{0}", context.Request.Url);

                                var response = client.GetAsync(url + path).Result;
                                if (response != null)
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
                                    done = true;
                                }
                            }

                            if (done)
                            {
                                context.Response.Flush();
                                context.Response.StatusCode = 200;
                                context.Response.StatusDescription = "OK";
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }
                        }
                    }
                    catch(ThreadAbortException)
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

        #endregion
    }




}
