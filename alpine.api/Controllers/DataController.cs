using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using alpine.database.Models;

namespace alpine.api.Controllers
{
     [Route("api/[controller]")]
     public class DataController : Controller
     {
          protected JsonResult SuccessMessage( object data = null )
          {
               return Json( new
               {
                    time = DateTime.UtcNow,
                    success = true,
                    results = data
               });
          }
     }
}
