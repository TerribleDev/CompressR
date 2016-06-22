using System;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CompressR.Exceptions;

namespace CompressR.WebApi
{
    public sealed class GzipAttribute : BaseCompressAttribute
    {
        public GzipAttribute(bool requireCompression = false)
            : base(requireCompression) { }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.CompressAction(actionExecutedContext, Constants.Gzip).Wait();
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            return base.CompressAction(actionExecutedContext, Constants.Gzip);
        }
    }
}