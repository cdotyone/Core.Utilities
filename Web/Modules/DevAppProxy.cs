using System;
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
                    if (string.IsNullOrEmpty(path) || path == @"/" || path.EndsWith(".aspx") || path.StartsWith("api/"))
                    {
                        return;
                    }                  

                    if (!path.StartsWith("/")) path = "/" + path;

                    HttpClient client = new HttpClient();
                    try
                    {
                        var url = DevAppProxySection.Current.DevUrl.TrimEnd('/');
                    
                        Trace.TraceInformation("Request To:{0}", context.Request.Url);

                        var response = client.GetAsync(url + path).Result;
                        if (response != null)
                        {
                            using (MemoryStream memStream = new MemoryStream())
                            {
                                Task task = response.Content.ReadAsStreamAsync().ContinueWith(t =>
                                {
                                    CopyStream(t.Result, memStream);
                                });

                                task.Wait();

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
        #endregion
    }




}
