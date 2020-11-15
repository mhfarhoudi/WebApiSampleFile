using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private HttpContent byteArrayContent;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public BufferedSingleFileUploadDb FileUpload { get; set; }

        public class BufferedSingleFileUploadDb
        {
            [Required]
            [Display(Name = "File")]
            public IFormFile FormFile { get; set; }
        }

        public void OnGet()
        {

        }

        public async void OnPost()
        {
            HttpClient _client = new HttpClient();

            var multipartContent = new MultipartFormDataContent();


            Products products = new Products() { Id = 1, Name = "Hossein" };

            var apiSerlize = JsonConvert.SerializeObject(products);

            var content = new StringContent(apiSerlize, Encoding.UTF8, "application/json");

            multipartContent.Add(content, "JsonDetails");

            string _FileName = FileUpload.FormFile.FileName;

            using (var fs = FileUpload.FormFile.OpenReadStream())
            {
                using (var streamContent = new StreamContent(fs))
                {
                    multipartContent.Add(streamContent, "files", _FileName);

                    var postResponse = await _client.PostAsync($"http://localhost:52864/api/Values/Upload", multipartContent);

                    if (postResponse.IsSuccessStatusCode)
                    {
                        await postResponse.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        throw new WebException($"The remote server returned unexpcted status code: {postResponse.StatusCode} - {postResponse.ReasonPhrase}.");
                    }
                }
            }


        }
    }
}
