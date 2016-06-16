using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace CompressR.MVC
{
    public static class CompressFactory
    {
        public static void Compress(string compression, System.Web.Mvc.ActionExecutingContext filterContext)
        {
            var context = filterContext.RequestContext.HttpContext;
            var compressionAccepted = context.Request.Headers.Get(Constants.AcceptEncoding)?.Split(',').Trim().Any(a => string.Equals(a, compression, StringComparison.OrdinalIgnoreCase)) ?? false;
            if (!compressionAccepted)
            {
                return;
            }
            HandleCompression(compression, filterContext);
        }

        public static void Compress(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            var context = filterContext.RequestContext.HttpContext;
            var compressionAlgorithm = context.Request.Headers.Get(Constants.AcceptEncoding)?.Split(',').Trim().Intersect(Constants.Compressors, StringComparer.OrdinalIgnoreCase)?.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(compressionAlgorithm))
            {
                HandleCompression(compressionAlgorithm, filterContext);
            }
        }

        private static void HandleCompression(string compression, System.Web.Mvc.ActionExecutingContext filterContext)
        {
            var context = filterContext.RequestContext.HttpContext;
            switch (compression)
            {
                case Constants.Gzip:
                    context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
                    context.Response.AppendHeader(Constants.ContentEncoding, Constants.Gzip);
                    context.Response.Cache.VaryByHeaders[Constants.AcceptEncoding] = true;
                    break;

                case Constants.Deflate:
                    context.Response.Filter = new DeflateStream(context.Response.Filter, CompressionMode.Compress);
                    context.Response.AppendHeader(Constants.ContentEncoding, Constants.Deflate);
                    context.Response.Cache.VaryByHeaders[Constants.AcceptEncoding] = true;
                    break;

                default:
                    break;
            }
        }
    }
}