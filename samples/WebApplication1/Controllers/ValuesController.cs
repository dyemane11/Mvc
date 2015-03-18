using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.JsonPatch;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpPost]
        public ObjectResult UpdateJsonPatch([FromBody]JsonPatchDocument<Customer> patchDoc)
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
