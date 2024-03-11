using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project0220.Models;

namespace Project0220.Controllers
{
    public class AllItemsController : Controller
    {
        private readonly ManualECommerceDBContext _context;

        public AllItemsController(ManualECommerceDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string bee)
        {

            var data = from o2 in _context.Products
                       select o2;

            if (!string.IsNullOrEmpty(bee))
            {
                data = data.Where(o => o.ProductName.Contains(bee));
            }

            return View(await data.ToListAsync());
        }
        [HttpPost]
        public IActionResult Search(string bee)
        {
            if (bee == null)
            {
                return BadRequest("Search string cannot be null.");
            }

            // Redirect to Index page with search query
            return RedirectToAction("Index", new { bee });
        }

        //[HttpPost]
        //public async Task<IActionResult> Index(string bee)
        //{

        //    var data = from o2 in _context.Products
        //               where o2.ProductName!.IndexOf(bee) > -1
        //               select o2;
        //    return View(await data.ToListAsync());
        //}
       
        public async Task<IActionResult> ItemDetails(int bee)
        {
            var z = bee;
            var data = from o2 in _context.Products
                       where o2.ProductID == bee
                       select o2;
            return View(await data.ToListAsync());

        }
        public IActionResult ItemDetails_2()
        {
            return View();
        }
        public IActionResult ItemDetails_3()
        {
            return View();
        }
        public IActionResult Sales()
        {
            return View();
        }
        public IActionResult New()
        {
            return View();
        }
    }
}
