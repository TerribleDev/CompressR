CompressR is a set of nuget packages that help you implement compression on your MVC, and WebApi Actions

`install-package CompressR.MVC5`

```csharp

public class HomeController : Controller
    {
        [Compress]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        [Gzip]
        public ActionResult About()
        {
           

            return View();
        }
    }

```

`install-package CompressR.WebApi`

```csharp

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

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }

```