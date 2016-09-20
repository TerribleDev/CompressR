using System;
using System.IO.Compression;
using System.Linq;
using System.Web.Mvc;

namespace CompressR.MVC
{
    public sealed class GzipAttribute : BaseCompressAttribute
    {
        public GzipAttribute(bool requireCompression = false)
            : base(requireCompression)
        {
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            CompressFactory.Compress(filterContext, RequireCompression, CompressionLevel);
        }
    }
}