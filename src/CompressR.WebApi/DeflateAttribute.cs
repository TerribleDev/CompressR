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
using CompressR.Exceptions;

namespace CompressR.WebApi
{
    public sealed class DeflateAttribute : BaseCompressAttribute
    {
        public DeflateAttribute(bool requireCompression = false)
            : base(requireCompression) { }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.CompressAction(actionExecutedContext, Constants.Deflate).Wait();
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if(cancellationToken.IsCancellationRequested)
            {
                return Task.FromResult(0);
            }
            return base.CompressAction(actionExecutedContext, Constants.Deflate);
        }
    }
}