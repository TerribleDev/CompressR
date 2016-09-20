using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;
using System.Web.Mvc;

namespace CompressR.MVC
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CompressAttribute : BaseCompressAttribute
    {
        public CompressAttribute(bool requireCompression = false)
            : base(requireCompression)
        {
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            CompressFactory.Compress(filterContext, RequireCompression, CompressionLevel);
        }
    }
}