using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Daisy.Data;
using Daisy.Models;
using Microsoft.EntityFrameworkCore;

namespace Daisy.Views.Flowers
{
    public class AddToCartModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddToCartModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Flower Flower { get; set; }
        [BindProperty]
        public Cart Cart { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Flower = await _context.Flower.SingleOrDefaultAsync(m => m.ID == id);
            if (Flower == null)
            {
                return NotFound();
            }
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Flower = await _context.Flower.FindAsync(id);

            if (Flower != null)
            {
                int quantityToBuy = Int32.Parse(Request.Form["quantityToBuy"]);
                int price = (int)Flower.Price;
                int Total2 = quantityToBuy * price;
                _context.Cart.AddRange(
                    new Cart
                    {
                        FlowerName = Flower.FlowerName,
                        Quantity = quantityToBuy,
                        Price = Flower.Price,
                        Total = Total2,
                        OrderBy = User.Identity.Name
                    });
                await _context.SaveChangesAsync();

            }
            return Page();
        }


    }
}