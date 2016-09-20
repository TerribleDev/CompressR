using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using CompressR.Exceptions;

namespace CompressR.MVC
{
    public static class CompressFactory
    {
        public static void Compress(System.Web.Mvc.ResultExecutedContext filterContext, bool requireCompression, string compression, CompressionLevel compressLevel = CompressionLevel.Optimal)
        {
            var context = filterContext.RequestContext.HttpContext;
            var compressionAccepted = context.Request.Headers.Get(Constants.AcceptEncoding)?.Split(',').Trim().Any(a => string.Equals(a, compression, StringComparison.OrdinalIgnoreCase)) ?? false;
            if(!compressionAccepted)
            {
                if(requireCompression)
                {
                    throw new CompressRException("Compression required but client did not send accept header");
                }
                else
                {
                    return;
                }
            }

            HandleCompression(compression, filterContext, compressLevel);
        }

        public static void Compress(System.Web.Mvc.ResultExecutedContext filterContext, bool requireCompression, CompressionLevel compressLevel = CompressionLevel.Optimal)
        {
            if(filterContext.Exception != null && !filterContext.ExceptionHandled)
            {
                return;
            }
            var context = filterContext.RequestContext.HttpContext;
            var compressionAlgorithm = context.Request.Headers.Get(Constants.AcceptEncoding)?.Split(',').Trim().Intersect(Constants.Compressors, StringComparer.OrdinalIgnoreCase)?.FirstOrDefault();
            if(!string.IsNullOrWhiteSpace(compressionAlgorithm))
            {
                HandleCompression(compressionAlgorithm, filterContext, compressLevel);
            }
            else if(requireCompression)
            {
                throw new CompressRException("Compression required but client did not send accept header");
            }
        }

        private static void HandleCompression(string compression, System.Web.Mvc.ResultExecutedContext filterContext, CompressionLevel compressLevel = CompressionLevel.Optimal)
        {
            var context = filterContext.RequestContext.HttpContext;
            HandleCompression(compression, filterContext.RequestContext.HttpContext, compressLevel);
        }

        private static void HandleCompression(string compression, System.Web.HttpContextBase context, CompressionLevel compressLevel)
        {
            switch(compression)
            {
                case Constants.Gzip:
                    context.Response.Filter = new GZipStream(context.Response.Filter, compressLevel);
                    context.Response.AppendHeader(Constants.ContentEncoding, Constants.Gzip);
                    context.Response.Cache.VaryByHeaders[Constants.AcceptEncoding] = true;
                    break;

                case Constants.Deflate:
                    context.Response.Filter = new DeflateStream(context.Response.Filter, compressLevel);
                    context.Response.AppendHeader(Constants.ContentEncoding, Constants.Deflate);
                    context.Response.Cache.VaryByHeaders[Constants.AcceptEncoding] = true;
                    break;

                default:
                    break;
            }
        }
    }
}