using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;

namespace CompressR.MVC
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class BaseCompressAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        protected bool RequireCompression { get; set; }

        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Optimal;

        protected BaseCompressAttribute(bool requireCompression = false)
        {
            RequireCompression = requireCompression;
        }
    }
}