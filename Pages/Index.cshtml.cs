using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Igo123.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace Igo123.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        [BindProperty]
        public User UserModel { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (UserModel != null)
            {
                var json = JsonSerializer.Serialize<User>(UserModel, new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    PropertyNameCaseInsensitive = true,
                });
                var stream = new MemoryStream(await GetUserInStream(json));
                Guid key = Guid.NewGuid();
                BlobContainerClient client = new BlobContainerClient(_configuration.GetConnectionString("AzureStorage"), "mywork");
                await client.UploadBlobAsync($"data-{key}.json", stream);
            }
            return RedirectToPage("Index");
        }

        private async Task<byte[]> GetUserInStream(string data)
        {
            if (data != "")
            {
                using (var ms = new MemoryStream())
                using (var sw = new StreamWriter(ms))
                {
                    await sw.WriteAsync(data);
                    await sw.FlushAsync();
                    return ms.ToArray();
                }
            }
            return null;
        }
    }
}
