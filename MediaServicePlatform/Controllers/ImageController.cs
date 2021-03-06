﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Core;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.IO;
using MediaServicePlatform.Utils;
using Microsoft.Extensions.Logging;

namespace MediaServicePlatform.Controllers
{
    [Route("[controller]/[action]")]
    public class ImageController : Controller
    {
        readonly WebConfig config;
        readonly IImageService imageService = null;
        readonly ILogger logger;
        public ImageController(IImageService imageService, ILogger<ImageController> logger, WebConfig webConfig)
        {
            this.config = webConfig;
            this.imageService = imageService;
            this.imageService.UseLogger(logger);
            this.logger = logger;
        }
        [HttpPost]
        public async Task<ActionResult<string>> Upload(IFormFile file)
        {
            if (file == null) {
                logger.LogWarning("did not get post request with a file.");
                return BadRequest("Invalid file format");
            }
            long size = file.Length;
            byte[] fileBuffer = new byte[size];
            MemoryStream fileStream = new MemoryStream(fileBuffer);
            await file.CopyToAsync(fileStream);
            UploadResult uploadResult = await imageService.UploadBinaryAsync(fileBuffer, file.FileName);
            if (uploadResult.Status == "ok" || uploadResult.Status == "duplicate")
            {
                logger.LogInformation($"new file: id={uploadResult.Id}, duplicate:{(uploadResult.Status=="duplicate")}");
                return Url.Action(action: "Download", controller: "Image", values: new { id = uploadResult.Id },
                    protocol: this.config.Protocol, host: HttpContext.Request.Host.ToString());
            } else
            {
                logger.LogWarning($"errorneous upload: {uploadResult.Status}");
                return BadRequest(uploadResult.Status);
            }

        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> Download(string id)
        {
            DownloadResult downloadResult = await imageService.DownloadBinaryAsync(id);
            if (downloadResult.Status == "not found")
            {
                logger.LogWarning($"id={id} not found");
                return NotFound();
            }
            if (downloadResult.Status == "ok")
            {
                string mimeType = MimeMapping.MimeUtility.GetMimeMapping(downloadResult.BlobName);
                logger.LogInformation($"downloaded id={id}, blobName={downloadResult.BlobName}");
                return File(downloadResult.FileBytes, mimeType, GetFileName(id, downloadResult.BlobName));
            } else
            {
                return BadRequest(downloadResult.Status);
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> Download(string id, [RequiredFromQuery] int xmin, [RequiredFromQuery] int xmax,
            [RequiredFromQuery] int ymin, [RequiredFromQuery] int ymax)
        {
            DownloadResult downloadResult = await imageService.DownloadAndCropBinaryAsync(id, xmin, xmax, ymin, ymax);
            if (downloadResult.Status == "not found")
            {
                logger.LogWarning($"id={id} not found");
                return NotFound();
            }
            if (downloadResult.Status == "ok")
            {
                string mimeType = MimeMapping.MimeUtility.GetMimeMapping(downloadResult.BlobName);
                logger.LogInformation($"downloaded id={id}, blobName={downloadResult.BlobName}, cropping=({xmin}, {xmax}, {ymin}, {ymax})");
                return File(downloadResult.FileBytes, mimeType, GetFileName(id, downloadResult.BlobName));
            }
            else
            {
                return BadRequest(downloadResult.Status);
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> Info(string id)
        {
            InfoResult infoResult = imageService.GetInfo(id);
            if (infoResult.Status == "not found")
            {
                return NotFound();
            }
            if (infoResult.Status == "ok")
            {
                return Json(infoResult.ImageInfo);
            } else
            {
                return BadRequest(infoResult.Status);
            }
        }
        [NonAction]
        private string GetFileName(string id, string blobName)
        {
            return "image_" + id + Path.GetExtension(blobName);
        }

        
    }


}
