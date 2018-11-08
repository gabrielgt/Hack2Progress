using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Company.Function
{
    public static class BlobTriggerCSharp
    {
        [FunctionName("BlobTriggerCSharp")]
        public static void Run(
            [BlobTrigger("original-images/{name}", Connection = "h2pdemosa_STORAGE")]Stream inputImage,
            [Blob("processed-images/{name}", FileAccess.Write, Connection = "h2pdemosa_STORAGE")] Stream outputImage,
            string name, 
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {inputImage.Length} Bytes");

            using (Image<Rgba32> image = Image.Load(inputImage))
            {
                image.Mutate(x => x.Grayscale());
                image.Save(outputImage, new JpegEncoder());
            }
        }
    }
}
