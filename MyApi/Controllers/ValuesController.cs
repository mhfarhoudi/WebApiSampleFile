using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApi.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public void Post([FromBody] string value, IFormFile file)
        {

        }

        [Route("Upload")]
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            try
            {
                var json = Request.Form;
                var file = json.Files[0];
                var _products = Request.Form["JsonDetails"];

                var _pro = JsonConvert.DeserializeObject<Products>(_products);

                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [Route("MultipleUploud")]
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult MultipleUploud()
        {
            try
            {
                var json = Request.Form;
                var fileOne = json.Files[0];
                var fileTwo = json.Files[1];
                var fileThree = json.Files[2];
                var _products = Request.Form["JsonDetails"];

                IFormFile[] formFiles = new IFormFile[] { fileOne, fileTwo, fileThree };

                var _pro = JsonConvert.DeserializeObject<Products>(_products);

                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                foreach (var item in formFiles)
                {
                    var fileName = $"{ContentDispositionHeaderValue.Parse(item.ContentDisposition).FileName.Trim('"')}.jpeg";
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        item.CopyTo(stream);
                    }
                }

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


    }
}
