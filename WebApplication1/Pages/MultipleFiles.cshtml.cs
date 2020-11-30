using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebApplication1.Common;

namespace WebApplication1.Pages
{
    public class MultipleFilesModel : PageModel
    {
        [BindProperty]
        public BufferedFileUploadDb FileUpload { get; set; }

        public class BufferedFileUploadDb
        {
            [Required]
            [Display(Name = "FileOne")]
            public IFormFile FormFileOne { get; set; }

            [Required]
            [Display(Name = "FileTwo")]
            public IFormFile FormFileTwo { get; set; }

            [Required]
            [Display(Name = "FileThree")]
            public IFormFile FormFileThree { get; set; }
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

            IFormFile[] formFiles = new IFormFile[] { FileUpload.FormFileOne, FileUpload.FormFileTwo, FileUpload.FormFileThree };

            //Solution One
            foreach (var item in formFiles)
            {
                var _bytes = item.GetBytes();
                var _file = new ByteArrayContent(_bytes);
                multipartContent.Add(_file, "files", item.Name);
            }

            var postResponse = await _client.PostAsync($"http://localhost:52864/api/Values/MultipleUploud", multipartContent);

            if (postResponse.IsSuccessStatusCode)
            {
                await postResponse.Content.ReadAsStringAsync();
            }

        }
    }
}

