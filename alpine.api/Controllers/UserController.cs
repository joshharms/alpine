using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace alpine.api.Controllers
{
     [Route("api/[controller]")]
     public class UserController : Controller
     {
          // POST api/user
          [HttpPost]
          public void Post([FromBody]string value)
          {
          }
     }
}
