using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Daisy.Controllers
{
    public class PromotionsController : Controller
    {

        public async Task<ActionResult> Index()
        {
            CloudBlobContainer container = GetCloudBlobContainer();

            var allBlobs = new List<Uri>();
            BlobContinuationToken blobContinuationToken = null;
            do
            {
                var response = await container.ListBlobsSegmentedAsync(blobContinuationToken);
                foreach (IListBlobItem blob in response.Results)
                {
                    if (blob.GetType() == typeof(CloudBlockBlob))
                    {
                        allBlobs.Add(blob.Uri);
                    }
                }
                blobContinuationToken = response.ContinuationToken;
            }
            while (blobContinuationToken != null);

            return View(allBlobs);
        }

        public IActionResult UploadPlan()
        {
            return View();
        }

        private CloudBlobContainer GetCloudBlobContainer()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            IConfigurationRoot Configuration = builder.Build();
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Configuration["ConnectionStrings:AzureStorageConnectionString-2"]);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("daisy-blob-container");
            container.CreateIfNotExistsAsync();
            return container;
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            foreach (var formFile in files)
            {
                if (formFile.Length <= 0)
                {
                    continue;
                }

                using (var stream = formFile.OpenReadStream())
                {
                    await UploadToBlob(formFile.FileName, null, stream);
                }

            }

            return RedirectToAction("Index");
        }

        private async Task<bool> UploadToBlob(string filename, byte[] imageBuffer = null, Stream stream = null)
        {
            CloudBlobContainer container = GetCloudBlobContainer();
            CloudBlockBlob blob = container.GetBlockBlobReference(filename);

            if (imageBuffer != null)
            {
                await blob.UploadFromByteArrayAsync(imageBuffer, 0, imageBuffer.Length);
            }
            else if (stream != null)
            {
                await blob.UploadFromStreamAsync(stream);
            }
            else
            {
                return false;
            }
            return true;
        }
    }
        
}