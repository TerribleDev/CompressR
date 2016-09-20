using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace CompressR.WebApi
{
    public sealed class CompressAttribute : BaseCompressAttribute
    {
        public CompressAttribute(bool requireCompression = false)
            : base(requireCompression) { }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.CompressAction(actionExecutedContext, Constants.Compressors).Wait();
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if(cancellationToken.IsCancellationRequested)
            {
                return Task.FromResult(0);
            }
            return base.CompressAction(actionExecutedContext, Constants.Compressors);
        }
    }
}