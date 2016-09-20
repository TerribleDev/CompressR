using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using CompressR.Exceptions;

namespace CompressR.WebApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class BaseCompressAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        protected bool RequireCompression { get; set; }
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Optimal;

        protected BaseCompressAttribute(bool requireCompression = false)
        {
            RequireCompression = requireCompression;
        }

        protected async Task CompressAction(HttpActionExecutedContext actionExecutedContext, params string[] compressors)
        {
            if(actionExecutedContext?.Exception != null)
            {
                return;
            }
            if(actionExecutedContext?.Response?.Content == null)
            {
                return;
            }
            var acceptedEncoding = actionExecutedContext
                .Response
                .RequestMessage
                .Headers
                .AcceptEncoding
                .Select(a => a.Value)
                .Intersect(compressors, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault();

            if(string.IsNullOrWhiteSpace(acceptedEncoding))
            {
                if(RequireCompression)
                {
                    throw new CompressRException("Compression required but client did not send accept header");
                }
                return;
            }

            actionExecutedContext.Response.Content = new CompressedContent(actionExecutedContext.Response.Content, acceptedEncoding, CompressionLevel);
        }
    }
}