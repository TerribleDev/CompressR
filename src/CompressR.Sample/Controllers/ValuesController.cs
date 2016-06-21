using CompressR.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CompressR.Sample.Controllers
{
    public class ValuesController : ApiController
    {
        [Compress]
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        [Compress]
        [HttpGet, Route("TestJsonSerialization")]
        public async Task<IHttpActionResult> TestJsonSerialization()
        {
            return Ok(new
            {
                A = 1,
                B = new string[] { "1", "A", "B" }

            });
        }

        [Compress(requireCompression: true)]
        [HttpGet, Route("TestJsonSerialization2")]
        public async Task<HttpResponseMessage> TestJsonSerialization2()
        {
            return Request.CreateResponse(new
            {
                A = 1,
                B = new string[] { "1", "A", "B" }

            });
        }

    }
}