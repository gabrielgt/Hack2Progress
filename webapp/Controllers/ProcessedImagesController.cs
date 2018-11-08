using System.Collections.Generic;
  using System.Threading.Tasks;
  using webapp.Storage;
  using Microsoft.AspNetCore.Mvc;

  namespace webapp.Controllers
  {
      [Route("api/[controller]")]
      public class ProcessedImagesController : Controller
      {
          [HttpGet("[action]")]
          public async Task<IEnumerable<string>> GetProcessedImages()
          {
              var processedImagesContainerName = "processed-images";

              var storage = new AzureStorageBlobClient(Startup.StorageConnectionString);
              var processedImages = await storage.GetUriBlobsNewestFirst(processedImagesContainerName);
              
              return processedImages;
          }
      }
  }
