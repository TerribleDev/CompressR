using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;
using System.Web.Mvc;

namespace CompressR.MVC
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CompressAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        private bool RequireCompression { get; set; }

        public CompressAttribute(bool requireCompression = false)
        {
            RequireCompression = requireCompression;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CompressFactory.Compress(filterContext, RequireCompression);
        }
    }
}