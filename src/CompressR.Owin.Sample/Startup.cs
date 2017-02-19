using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;

namespace CompressR.Owin.Sample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use<GzipMiddleware>();
            app.Run(context =>
            {
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Hello World!");
            });
        }
    }
}