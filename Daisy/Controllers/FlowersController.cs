using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Daisy.Models;
using Daisy.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Daisy.Views
{
    public class FlowersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlowersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Flower> Flower { get; set; }
        public SelectList Types { get; set; }
        public string FlowerTypes { get; set; }
        public string SearchString { get; set; }

        [BindProperty]
        public Flower Flowers { get; set; }
        [BindProperty]
        public Cart Cart { get; set; }
        [BindProperty]
        public FlowerUpload FlowerUpload { get; set; }

        // GET: Flowers
        public async Task<IActionResult> Index(string searchString, string FlowerType)
        {
            // Use LINQ to get list of genres.
            IQueryable<string> TypeQuery = from m in _context.Flower
                                           orderby m.Type
                                           select m.Type;
            //Flower = await _context.Flower.ToListAsync();
            var Flowers = from m in _context.Flower
                          select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                Flowers = Flowers.Where(s => s.FlowerName.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(FlowerType))
            {
                Flowers = Flowers.Where(x => x.Type == FlowerType);
            }

            IEnumerable<SelectListItem> items = new SelectList(await TypeQuery.Distinct().ToListAsync());
            ViewBag.FlowerType = items;
            Flower = await Flowers.ToListAsync();
            return View(Flower);
        }


        // GET: Flowers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Flower = await _context.Flower
                .FirstOrDefaultAsync(m => m.ID == id);
            if (Flower == null)
            {
                return NotFound();
            }

            return View(Flower);
        }

        // GET: Flowers/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        private CloudBlobContainer GetCloudBlobContainer()
        {
            //to access your appsettings.json file to get the connection string info
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            IConfigurationRoot Configuration = builder.Build();
            CloudStorageAccount storageAccount =
            // is to get the connection info
            CloudStorageAccount.Parse(Configuration[
                    "ConnectionStrings:AzureStorageConnectionString-1"]);
            // to build the blob container
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            //give the name for the container that you want to refer
            CloudBlobContainer container =
                blobClient.GetContainerReference("Flower-blob-container");
            return container;
        }


        public CloudBlockBlob UploadImage(string imageName, IFormFile files)
        {
            CloudBlobContainer container = GetCloudBlobContainer();
            CloudBlockBlob blob = container.GetBlockBlobReference("FlowerImages/" + imageName + DateTime.Now);
            using (var fileStream = files.OpenReadStream())
            {
                blob.UploadFromStreamAsync(fileStream).Wait();
            }
            return blob;
        }

        // POST: Flowers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FlowerName,FlowerProducedDate,Type,Price,Quantity")] FlowerUpload FlowerUpload, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewData["ImageError"] = "No image uploaded";
                return View();
            }
            var image = UploadImage(FlowerUpload.FlowerName, file);
            var Flower = new Flower()
            {
                FlowerName = FlowerUpload.FlowerName,
                FlowerProducedDate = FlowerUpload.FlowerProducedDate,
                Type = FlowerUpload.Type,
                Price = FlowerUpload.Price,
                Quantity = FlowerUpload.Quantity,
                Image = image.Uri.AbsoluteUri,
                ImageRef = image.Name
            };
            _context.Add(Flower);
            await _context.SaveChangesAsync();
            ViewData["Message"] = "New Flower added.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Flowers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Flower = await _context.Flower.FindAsync(id);
            if (Flower == null)
            {
                return NotFound();
            }
            return View(Flower);
        }

        // POST: Flowers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FlowerName,FlowerProducedDate,Type,Price,Quantity,Image")] Flower Flower)
        {
            if (id != Flower.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Flower);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlowerExists(Flower.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Flower);
        }

        // GET: Flowers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Flower = await _context.Flower
                .FirstOrDefaultAsync(m => m.ID == id);
            if (Flower == null)
            {
                return NotFound();
            }

            return View(Flower);
        }

        // POST: Flowers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Flower = await _context.Flower.FindAsync(id);
            CloudBlobContainer container = GetCloudBlobContainer();
            if (Flower.ImageRef != null)
            {

                CloudBlockBlob blob = container.GetBlockBlobReference(Flower.ImageRef);
                blob.DeleteAsync().Wait();

            }
            _context.Flower.Remove(Flower);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlowerExists(int id)
        {
            return _context.Flower.Any(e => e.ID == id);
        }

        public async Task<IActionResult> AddToCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Flower = await _context.Flower
                .FirstOrDefaultAsync(m => m.ID == id);
            if (Flower == null)
            {
                return NotFound();
            }

            return View(Flower);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int id)
        {
            if (id != Flowers.ID)
            {
                return NotFound();
            }

            Flowers = await _context.Flower.FindAsync(id);

            if (Flowers != null)
            {
                int quantityToBuy = Int32.Parse(Request.Form["quantityToBuy"]);
                Flowers.Quantity = Flowers.Quantity - quantityToBuy;

                if (quantityToBuy < Flowers.Quantity && quantityToBuy != 0)
                {
                    int price = (int)Flowers.Price;
                    int Total2 = quantityToBuy * price;
                    _context.Update(Flowers);
                    _context.Cart.AddRange(
                        new Cart
                        {
                            FlowerName = Flowers.FlowerName,
                            Quantity = quantityToBuy,
                            Price = Flowers.Price,
                            Total = Total2,
                            OrderBy = User.Identity.Name
                        });

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {

                }

            }
            return View(Flower);
        }
    }
        
}
