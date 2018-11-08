using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;

namespace webapp.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<WeatherForecast>> WeatherForecasts()
        {
            if (Startup.BingSearchApiKey == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Registra la key Azure:BingSearchApiKey");
            }

            var rng = new Random();
            return Ok(Enumerable.Range(1, 5).Select(index =>
            {
                var summary = Summaries[rng.Next(Summaries.Length)];
                var searchTerm = $"weather {summary}";

                return new WeatherForecast
                {
                    DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = summary,
                    ImageUrl = SampleDataController.SearchImagesWithSdk(searchTerm)[index].ThumbnailUrl
                };
            }));
        }

        private static IList<ImageObject> SearchImagesWithSdk(string searchTerm)
        {
            var subscriptionKey = Startup.BingSearchApiKey;
            var client = new ImageSearchAPI(new ApiKeyServiceClientCredentials(subscriptionKey));
            var imageResults = client.Images.SearchAsync(query: searchTerm).Result;
            return imageResults.Value;
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }
            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
            public string ImageUrl { get; set; }
        }
    }
}
