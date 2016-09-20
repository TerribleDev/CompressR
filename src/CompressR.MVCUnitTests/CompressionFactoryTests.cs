using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using Xunit;

namespace CompressR.MVCUnitTests
{
    public class CompressionFactoryTests
    {
        [Fact]
        public void ShouldThrowWhenRequired()
        {
            var request = new Mock<HttpRequestBase>();
            request.Setup(a => a.Headers).Returns(new System.Collections.Specialized.NameValueCollection() { });
            var httpContext = new Moq.Mock<HttpContextBase>();
            httpContext.Setup(a => a.Request).Returns(request.Object);
            Assert.Throws(typeof(CompressR.Exceptions.CompressRException), () =>
            {
                CompressR.MVC.CompressFactory.Compress(new System.Web.Mvc.ResultExecutedContext()
                {
                    HttpContext = httpContext.Object
                }, true);
            });
        }

        [Fact]
        public void ShouldNotThrowButAlsoNotCompressIfFalse()
        {
            var request = new Mock<HttpRequestBase>();
            request.Setup(a => a.Headers).Returns(new System.Collections.Specialized.NameValueCollection() { });
            var httpContext = new Moq.Mock<HttpContextBase>();
            httpContext.Setup(a => a.Request).Returns(request.Object);
            var response = new Mock<HttpResponseBase>();
            httpContext.Setup(a => a.Response).Returns(response.Object);
            var contextObject = httpContext.Object;
            CompressR.MVC.CompressFactory.Compress(new System.Web.Mvc.ResultExecutedContext()
            {
                HttpContext = contextObject
            }, false);
            Assert.Null(contextObject.Response.Filter);
        }

        [Fact]
        public void ShouldGzipWhenRequested()
        {
            var request = new Mock<HttpRequestBase>();
            request.Setup(a => a.Headers).Returns(new System.Collections.Specialized.NameValueCollection() { ["Accept-Encoding"] = "gzip" });
            var requestContextBase = new Mock<HttpContextBase>();
            requestContextBase.Setup(a => a.Request).Returns(request.Object);
            var requestContext = new Mock<RequestContext>();
            requestContext.Setup(a => a.HttpContext).Returns(requestContextBase.Object);
            var cachePolicy = new Mock<HttpCachePolicyBase>();
            cachePolicy.Setup(a => a.VaryByHeaders).Returns(new HttpCacheVaryByHeaders());
            //var httpContext = new Moq.Mock<ActionExecutingContext>();
            var response = new Mock<HttpResponseBase>();
            response.Setup(a => a.Headers).Returns(new System.Collections.Specialized.NameValueCollection());
            response.SetupProperty(a => a.Filter, new MemoryStream(Encoding.UTF8.GetBytes("awesome")) { });
            response.Setup(a => a.Cache).Returns(cachePolicy.Object);
            var responseContext = new Mock<HttpContextBase>();
            responseContext.Setup(a => a.Response).Returns(response.Object);

            requestContextBase.Setup(a => a.Response).Returns(response.Object);
            var httpContext = new ResultExecutedContext()
            {
                RequestContext = new System.Web.Routing.RequestContext()
                {
                    HttpContext = requestContextBase.Object
                }
            };

            //httpContext.Setup(a=>a.RequestContext.Filter).Returns(new MemoryStream(Encoding.UTF8.GetBytes("awesome")) { } );
            //var response = new Mock<HttpResponseBase>();
            //httpContext.Setup(a => a.Response).Returns(response.Object);
            //var contextObject = httpContext;
            CompressR.MVC.CompressFactory.Compress(httpContext, false);
            Assert.IsType<GZipStream>(httpContext.RequestContext.HttpContext.Response.Filter);
        }

        [Fact]
        public void ShouldNotGzipWhenExceptionNotHandled()
        {
            var request = new Mock<HttpRequestBase>();
            request.Setup(a => a.Headers).Returns(new System.Collections.Specialized.NameValueCollection() { ["Accept-Encoding"] = "gzip" });
            var requestContextBase = new Mock<HttpContextBase>();
            requestContextBase.Setup(a => a.Request).Returns(request.Object);
            var requestContext = new Mock<RequestContext>();
            requestContext.Setup(a => a.HttpContext).Returns(requestContextBase.Object);
            var cachePolicy = new Mock<HttpCachePolicyBase>();
            cachePolicy.Setup(a => a.VaryByHeaders).Returns(new HttpCacheVaryByHeaders());
            //var httpContext = new Moq.Mock<ActionExecutingContext>();
            var response = new Mock<HttpResponseBase>();
            response.Setup(a => a.Headers).Returns(new System.Collections.Specialized.NameValueCollection());
            response.SetupProperty(a => a.Filter, new MemoryStream(Encoding.UTF8.GetBytes("awesome")) { });
            response.Setup(a => a.Cache).Returns(cachePolicy.Object);
            var responseContext = new Mock<HttpContextBase>();
            responseContext.Setup(a => a.Response).Returns(response.Object);

            requestContextBase.Setup(a => a.Response).Returns(response.Object);
            var httpContext = new ResultExecutedContext()
            {
                RequestContext = new System.Web.Routing.RequestContext()
                {
                    HttpContext = requestContextBase.Object
                },
                Exception = new Exception(),
                ExceptionHandled = false
            };
            CompressR.MVC.CompressFactory.Compress(httpContext, false);
            Assert.IsNotType<GZipStream>(httpContext.RequestContext.HttpContext.Response.Filter);
        }

        [Fact]
        public void ShouldGzipWhenExceptionHandled()
        {
            var request = new Mock<HttpRequestBase>();
            request.Setup(a => a.Headers).Returns(new System.Collections.Specialized.NameValueCollection() { ["Accept-Encoding"] = "gzip" });
            var requestContextBase = new Mock<HttpContextBase>();
            requestContextBase.Setup(a => a.Request).Returns(request.Object);
            var requestContext = new Mock<RequestContext>();
            requestContext.Setup(a => a.HttpContext).Returns(requestContextBase.Object);
            var cachePolicy = new Mock<HttpCachePolicyBase>();
            cachePolicy.Setup(a => a.VaryByHeaders).Returns(new HttpCacheVaryByHeaders());
            //var httpContext = new Moq.Mock<ActionExecutingContext>();
            var response = new Mock<HttpResponseBase>();
            response.Setup(a => a.Headers).Returns(new System.Collections.Specialized.NameValueCollection());
            response.SetupProperty(a => a.Filter, new MemoryStream(Encoding.UTF8.GetBytes("awesome")) { });
            response.Setup(a => a.Cache).Returns(cachePolicy.Object);
            var responseContext = new Mock<HttpContextBase>();
            responseContext.Setup(a => a.Response).Returns(response.Object);

            requestContextBase.Setup(a => a.Response).Returns(response.Object);
            var httpContext = new ResultExecutedContext()
            {
                RequestContext = new System.Web.Routing.RequestContext()
                {
                    HttpContext = requestContextBase.Object
                },
                Exception = new Exception(),
                ExceptionHandled = true
            };
            CompressR.MVC.CompressFactory.Compress(httpContext, false);
            Assert.IsType<GZipStream>(httpContext.RequestContext.HttpContext.Response.Filter);
        }

        [Fact]
        public void ShouldDeflateWhenRequested()
        {
            var request = new Mock<HttpRequestBase>();
            request.Setup(a => a.Headers).Returns(new System.Collections.Specialized.NameValueCollection() { ["Accept-Encoding"] = "deflate" });
            var requestContextBase = new Mock<HttpContextBase>();
            requestContextBase.Setup(a => a.Request).Returns(request.Object);
            var requestContext = new Mock<RequestContext>();
            requestContext.Setup(a => a.HttpContext).Returns(requestContextBase.Object);
            var cachePolicy = new Mock<HttpCachePolicyBase>();
            cachePolicy.Setup(a => a.VaryByHeaders).Returns(new HttpCacheVaryByHeaders());
            //var httpContext = new Moq.Mock<ActionExecutingContext>();
            var response = new Mock<HttpResponseBase>();
            response.Setup(a => a.Headers).Returns(new System.Collections.Specialized.NameValueCollection());
            response.SetupProperty(a => a.Filter, new MemoryStream(Encoding.UTF8.GetBytes("awesome")) { });
            response.Setup(a => a.Cache).Returns(cachePolicy.Object);
            var responseContext = new Mock<HttpContextBase>();
            responseContext.Setup(a => a.Response).Returns(response.Object);

            requestContextBase.Setup(a => a.Response).Returns(response.Object);
            var httpContext = new ResultExecutedContext()
            {
                RequestContext = new System.Web.Routing.RequestContext()
                {
                    HttpContext = requestContextBase.Object
                }
            };

            //httpContext.Setup(a=>a.RequestContext.Filter).Returns(new MemoryStream(Encoding.UTF8.GetBytes("awesome")) { } );
            //var response = new Mock<HttpResponseBase>();
            //httpContext.Setup(a => a.Response).Returns(response.Object);
            //var contextObject = httpContext;
            CompressR.MVC.CompressFactory.Compress(httpContext, false);
            Assert.IsType<DeflateStream>(httpContext.RequestContext.HttpContext.Response.Filter);
        }
    }
}