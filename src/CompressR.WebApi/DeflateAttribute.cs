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
    public sealed class DeflateAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        private bool RequireCompression { get; set; }

        public DeflateAttribute(bool requireCompression = false)
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
                .Any(a => a.Equals(Constants.Deflate, StringComparison.OrdinalIgnoreCase));

            if (!acceptedEncoding)
            {
                return;
            }
            actionExecutedContext.Response.Content = new CompressedContent(actionExecutedContext.Response.Content, Constants.Deflate);
        }

        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var acceptedEncoding = actionExecutedContext
            .Response
            .RequestMessage
            .Headers
            .AcceptEncoding
            .Select(a => a.Value)
            .Any(a => a.Equals(Constants.Deflate, StringComparison.OrdinalIgnoreCase));

            if (!acceptedEncoding)
            {
                return;
            }
            actionExecutedContext.Response.Content = new CompressedContent(actionExecutedContext.Response.Content, Constants.Deflate);
        }
    }
}