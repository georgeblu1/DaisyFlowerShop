using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Daisy.Data;
using Daisy.Models;

namespace Daisy.Views
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        // GET: Carts1
        public async Task<IActionResult> Index()
        {
            var carts = _context.Cart.Where(s => s.OrderBy == User.Identity.Name);

            return View(await carts.ToListAsync());
        }

        public IActionResult AddAddress()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment()
        {
            List<string> iList = new List<string>();

            foreach (var item in _context.Cart.Where(s => s.OrderBy == User.Identity.Name))
            {
                if (item != null)
                {
                    iList.Add(item.FlowerName + " " + item.Quantity.ToString());
                    _context.Cart.Remove(item);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            var result = String.Join(", ", iList.ToArray());
            if (result != " ")
            {
                _context.Order.AddRange(
                new Order
                {
                    OrderDate = DateTime.Now,
                    OrderBy = User.Identity.Name,
                    FlowerBought = result,
                    Total = _context.Cart.Sum(s => s.Total),
                    Address = Request.Form["Address"],
                    Status = "Processing"
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        // GET: Carts1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Carts1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FlowerName,Quantity,Price,Total,OrderBy")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                await _context.SaveChangesAsync();
                _context.Add(cart);
                return RedirectToAction(nameof(Index));
            }
            return View(cart);
        }

        // GET: Carts1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }

        // POST: Carts1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FlowerName,Quantity,Price,Total,OrderBy")] Cart cart)
        {
            if (id != cart.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.ID))
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
            return View(cart);
        }

        // GET: Carts1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Cart.FindAsync(id);
            Flower Flower = _context.Flower.Where(x => x.FlowerName == cart.FlowerName).FirstOrDefault();
            Flower.Quantity = Flower.Quantity + cart.Quantity;
            _context.Flower.Update(Flower);
            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.ID == id);
        }
    }

}
