using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using webapp.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;
using Microsoft.AspNetCore.Http;

namespace webapp.Controllers
{
    [Route("api/[controller]")]
    public class BingApiSearchController : Controller
    {
        [HttpGet("[action]")]
        public ActionResult<IEnumerable<string>> SearchImages(string searchTerm)
        {
            if (Startup.BingSearchApiKey == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Registra la key Azure:BingSearchApiKey");
            }

            var images = BingApiSearchController.SearchImagesWithSdk(searchTerm);
            var urls = images.Select(img => img.ThumbnailUrl).ToList();

            return Ok(urls);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendSelectedImages([FromBody] string[] selectedImages)
        {
            if (Startup.StorageConnectionString == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Registra la key Azure:StorageConnectionString");
            }

            var httpClient = new HttpClient();
            var storage = new AzureStorageBlobClient(Startup.StorageConnectionString);
            var originalImagesContainerName = "original-images";

            foreach (var selectedImage in selectedImages)
            {
                var fileName = $"{Guid.NewGuid()}.jpg";

                var downloadedImage = await httpClient.GetByteArrayAsync(selectedImage);
                await storage.AddFileAsync(originalImagesContainerName, fileName, downloadedImage);
            }

            return Ok();
        }

        private static IList<ImageObject> SearchImagesWithSdk(string searchTerm)
        {
            var subscriptionKey = Startup.BingSearchApiKey;
            var client = new ImageSearchAPI(new ApiKeyServiceClientCredentials(subscriptionKey));
            var imageResults = client.Images.SearchAsync(query: searchTerm).Result;
            return imageResults.Value;
        }
    }
}
