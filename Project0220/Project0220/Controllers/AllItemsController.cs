using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project0220.myModels;
using Project0220.ViewModel;

namespace Project0220.Controllers
{
    public class AllItemsController : Controller
    {
        private readonly ScaffoldEcommerceDbContext _contextNew;


        public AllItemsController( ScaffoldEcommerceDbContext contextNew)
        {
            _contextNew = contextNew;
        }

        public async Task<IActionResult> Index(string bee)
        {

            var data = from o2 in _contextNew.Products
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
            var data = await _contextNew.Products
                            .Where(o2 => o2.ProductId == bee)
                            .ToListAsync();


            var category = await _contextNew.Categories
                                            .Where(c => c.CategoryId == bee)
                                            .ToListAsync();
            var viewModel = new ItemDetailsViewModel
            {
                Products = data,
                Category = category
            };
            return View(viewModel);

        }
   
        
        
            public async Task<IActionResult> Clearance(string bee)
            {

                var data = from o2 in _contextNew.Products
                           where o2.SpecialZoneType == "出清"
                           select o2;

                if (!string.IsNullOrEmpty(bee))
                {
                    data = data.Where(o => o.ProductName.Contains(bee));
                }

                return View(await data.ToListAsync());
            }




        public async Task<IActionResult> New(string bee)
        {

            var data = from o2 in _contextNew.Products
                       where o2.SpecialZoneType == "新品"
                       select o2;

            if (!string.IsNullOrEmpty(bee))
            {
                data = data.Where(o => o.ProductName.Contains(bee));
            }

            return View(await data.ToListAsync());
        }
    }
}
