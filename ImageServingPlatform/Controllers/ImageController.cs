using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Core;
using Microsoft.Extensions.Options;

namespace ImageServingPlatform.Controllers
{
    [Route("[controller]/[action]")]
    public class ImageController : Controller
    {
        string greeting;
        public ImageController(IOptions<Fuck> fuck)
        {
            greeting = fuck.Value.shit;
        }
        [HttpPost]
        public ActionResult<String> Upload()
        {
            return Ok(new { fuck = greeting, shit = 2});
        }
    }
}
