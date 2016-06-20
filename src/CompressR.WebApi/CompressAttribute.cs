using CompressR.Exceptions;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CompressR.WebApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CompressAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        private bool RequireCompression { get; set; }

        public CompressAttribute(bool requireCompression = false)
        {
            RequireCompression = requireCompression;
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var acceptedEncoding = actionExecutedContext
                .Response
                .RequestMessage
                .Headers
                .AcceptEncoding
                .Select(a => a.Value)
                .Intersect(Constants.Compressors, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(acceptedEncoding))
            {
                if (RequireCompression)
                    throw new CompressRException("Compression required but client did not send accept header");
                else
                    return;
            }

            actionExecutedContext.Response.Content = new CompressedContent(actionExecutedContext.Response.Content, acceptedEncoding);
        }

        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var acceptedEncoding = actionExecutedContext
                .Response
                .RequestMessage
                .Headers
                .AcceptEncoding
                .Select(a => a.Value)
                .Intersect(Constants.Compressors, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(acceptedEncoding))
            {
                return;
            }

            actionExecutedContext.Response.Content = new CompressedContent(actionExecutedContext.Response.Content, acceptedEncoding);
        }
    }
}