using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace CompressR.Owin
{
    public struct Constants
    {
        public const string Gzip = "gzip";
        public const string Deflate = "deflate";
        public const string ContentEncoding = "Content-Encoding";
        public const string AcceptEncoding = "Accept-Encoding";
        public static readonly string[] Compressors = { Gzip, Deflate };
    }

    public class GzipMiddleware : OwinMiddleware
    {
        public GzipMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public async override Task Invoke(IOwinContext context)
        {
            if(!context.Request.Headers.Keys.Contains(Constants.AcceptEncoding))
            {
                await Next.Invoke(context);
            }
            var types = context.Request.Headers[Constants.AcceptEncoding].Split(',');
            if(types.Contains(Constants.Gzip, StringComparer.OrdinalIgnoreCase))
            {
                var responseStream = context.Response.Body;
                using(var memStream = new MemoryStream())
                {
                    context.Response.Body = memStream;
                    await Next?.Invoke(context);
                    context.Response.Headers[Constants.ContentEncoding] = Constants.Gzip;
                    using(var gzipStream = new GZipStream(responseStream, CompressionLevel.Optimal))
                    {
                        memStream.WriteTo(gzipStream);
                        await gzipStream.FlushAsync();
                    }

                    await responseStream.FlushAsync();

                }
            }
        }
    }
}
