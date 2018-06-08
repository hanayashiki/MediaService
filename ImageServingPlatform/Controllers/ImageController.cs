using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Core;
using Microsoft.Extensions.Options;

namespace MediaServicePlatform.Controllers
{
    [Route("[controller]/[action]")]
    public class ImageController : Controller
    {
        readonly MediaServiceConfiguration config = null;
        readonly IImageService imageService = null;
        public ImageController(IOptions<MediaServiceConfiguration> config)
        {
            this.config = config.Value;
            this.imageService = this.config.ImageService;
        }
        [HttpPost]
        public ActionResult<String> Upload()
        {
            return Ok();
        }
    }
}
