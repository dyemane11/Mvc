using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.JsonPatch;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MvcSample.Web.Controllers
{
    [Route("api/patch")]
    public class JsonPatchController : Controller
    {
        [HttpPost]
        public ObjectResult UpdateJsonPatch(int id, [FromBody]JsonPatchDocument<Customer> patchDoc)
        {
            Customer c = new Customer
            {
                Name = "Test",
                Order = new Order()
            };
            patchDoc.ApplyTo(c);

            return new ObjectResult(c);
        }

        public class Customer
        {
            public int Id { get; set; }
            [JsonIgnore]
            public string Name { get; set; }
            public Order Order { get; set; }
        }

        public class Order
        {
            public string OrderName { get; set; }

        }
    }
}
