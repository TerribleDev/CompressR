using CompressR.Exceptions;
using System;
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
    public sealed class GzipAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        private bool RequireCompression { get; set; }

        public GzipAttribute(bool requireCompression = false)
        {
            RequireCompression = requireCompression;
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            OnActionExecutedAsync(actionExecutedContext, CancellationToken.None).Wait();
        }

        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if(actionExecutedContext.Response.Content == null)
            {
                return;
            }
            var acceptedEncoding = actionExecutedContext
            .Response
            .RequestMessage
            .Headers
            .AcceptEncoding
            .Select(a => a.Value)
            .Any(a => a.Equals(Constants.Gzip, StringComparison.OrdinalIgnoreCase));


            if (!acceptedEncoding && RequireCompression)
            {
                throw new CompressRException("Compression required but client did not send accept header");
            }

            if (!acceptedEncoding)
            {
                return;
            }

            actionExecutedContext.Response.Content = new CompressedContent(actionExecutedContext.Response.Content, Constants.Gzip);
        }
    }
}