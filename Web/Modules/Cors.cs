using System;
using System.Globalization;
using System.Linq;
using System.Web;
using Civic.Core.Framework.Configuration;

namespace Civic.Core.Framework.Web.Modules
{
	public class Cors : IHttpModule
	{
		/// <summary>
		/// Initializes a module and prepares it to handle requests.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application </param>
		public void Init(HttpApplication context)
		{
			context.BeginRequest += context_BeginRequest;
		}

		/// <summary>
		/// adds CORS compatible http headers 
		/// </summary>
		void context_BeginRequest(object sender, EventArgs e)
		{
			HttpContext context = HttpContext.Current;

			var refereerUri = context.Request.UrlReferrer;
			if (refereerUri == null && !string.IsNullOrEmpty(context.Request.Headers["origin"]))
			{
				refereerUri = new Uri(context.Request.Headers["origin"]);
			}

			if (refereerUri != null)
			{
				var config = CorsSection.Current;

				// does the configuration require the HTTP Method = OPTIONS
				if(config.RequireOptions && string.Compare("OPTIONS", context.Request.HttpMethod,StringComparison.InvariantCultureIgnoreCase)!=0) return;

				// now check for authorized domains
				var response = context.Response;
				if (config.Allowed(refereerUri.Host))
				{
					if (refereerUri.Port == 80 || refereerUri.Port == 443)
					{
						response.AppendHeader("Access-Control-Allow-Origin",
													  refereerUri.Scheme + "://" + refereerUri.Host);
					}
					else
					{
						response.AppendHeader("Access-Control-Allow-Origin",
													  refereerUri.Scheme + "://" + refereerUri.Host + ":" +
													  refereerUri.Port);
					}

					if (config.MaxAge > 0) response.AppendHeader("Access-Control-Max-Age", config.MaxAge.ToString(CultureInfo.InvariantCulture));
					if (config.OutputAllowCredentials) response.AppendHeader("Access-Control-Allow-Credentials", "true");
					if (!string.IsNullOrEmpty(config.AllowedHeaders)) response.AppendHeader("Access-Control-Allow-Headers", config.AllowedHeaders);
					if (!string.IsNullOrEmpty(config.AllowedMethods)) response.AppendHeader("Access-Control-Allow-Methods", config.AllowedMethods);
				}

				if (string.Compare("OPTIONS", context.Request.HttpMethod, StringComparison.InvariantCultureIgnoreCase)==0)
				{
					response.Write("");
					response.Flush();
					response.End();
				}
			}
		}

		/// <summary>
		/// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
		/// </summary>
		public void Dispose()
		{
		}
	}
}
