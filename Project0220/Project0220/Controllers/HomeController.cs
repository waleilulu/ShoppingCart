using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project0220.Models;
using Project0220.myModels;
using System.Diagnostics;

namespace Project0220.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly ScaffoldEcommerceDbContext _contextNew;


        public HomeController(ILogger<HomeController> logger,ScaffoldEcommerceDbContext contextNew)
		{
            _contextNew = contextNew;
            _logger = logger;
		}

		public async Task<IActionResult> Index()
		{
            var data = await _contextNew.Products
                             .Where(o2 => o2.ProductId >= 5 && o2.ProductId <= 8)
                             .ToListAsync();
            var id = Convert.ToInt32(HttpContext.Request.Cookies["membercookie"]);
            var cartQuantity = _contextNew.CartItems.Where(c => c.CustomerID == id).Sum(c => c.Quantity);
            HttpContext.Session.SetInt32("cartQuantity", cartQuantity);
            ViewData["CartQuantity"] = cartQuantity;

            return View(data);
        }
        

        public IActionResult Privacy()
		{
			return View();
		}
        public async Task<IActionResult> Sofa(string bee)
        {
            // 查詢CategoryId為1的商品
            var data = from o2 in _contextNew.Products
                       where o2.CategoryId == 1
                       select o2;

            // 如果bee不為空，進一步過濾ProductName包含bee的商品
            if (!string.IsNullOrEmpty(bee))
            {
                data = data.Where(o => o.ProductName.Contains(bee));
            }

            // 執行查詢，並將結果轉換為列表
            return View(await data.ToListAsync());
        }
        public async Task<IActionResult> Cabinet(string bee)
        {

            var data = from o2 in _contextNew.Products
                       where o2.CategoryId == 2
                       select o2;

            // 如果bee不為空，進一步過濾ProductName包含bee的商品
            if (!string.IsNullOrEmpty(bee))
            {
                data = data.Where(o => o.ProductName.Contains(bee));
            }

            // 執行查詢，並將結果轉換為列表
            return View(await data.ToListAsync());
        }
        public async Task<IActionResult> Bedding(string bee)
        {

            var data = from o2 in _contextNew.Products
                       where o2.CategoryId == 3
                       select o2;

            // 如果bee不為空，進一步過濾ProductName包含bee的商品
            if (!string.IsNullOrEmpty(bee))
            {
                data = data.Where(o => o.ProductName.Contains(bee));
            }

            // 執行查詢，並將結果轉換為列表
            return View(await data.ToListAsync());
        }
        public async Task<IActionResult> Decorations(string bee)
        {

            var data = from o2 in _contextNew.Products
                       where o2.CategoryId == 4
                       select o2;

            // 如果bee不為空，進一步過濾ProductName包含bee的商品
            if (!string.IsNullOrEmpty(bee))
            {
                data = data.Where(o => o.ProductName.Contains(bee));
            }

            // 執行查詢，並將結果轉換為列表
            return View(await data.ToListAsync());
        }
        public async Task<IActionResult> Bath(string bee)
        {

            var data = from o2 in _contextNew.Products
                       where o2.CategoryId == 5
                       select o2;

            // 如果bee不為空，進一步過濾ProductName包含bee的商品
            if (!string.IsNullOrEmpty(bee))
            {
                data = data.Where(o => o.ProductName.Contains(bee));
            }

            // 執行查詢，並將結果轉換為列表
            return View(await data.ToListAsync());
        }

        public async Task<IActionResult> NewHome(string bee)
        {

            var data = from o2 in _contextNew.Products
                       where o2.SpecialZoneType == "新家"
                       select o2;

            if (!string.IsNullOrEmpty(bee))
            {
                data = data.Where(o => o.ProductName.Contains(bee));
            }
            var ret = data.ToList();
            return View(await data.ToListAsync());
        }


        public async Task<IActionResult> Family(string bee)
        {

            var data = from o2 in _contextNew.Products
                       where o2.SpecialZoneType == "小家庭"
                       select o2;

            if (!string.IsNullOrEmpty(bee))
            {
                data = data.Where(o => o.ProductName.Contains(bee));
            }

            return View(await data.ToListAsync());
        }

        public async Task<IActionResult> Bedroom(string bee)
        {

            var data = from o2 in _contextNew.Products
                       where o2.SpecialZoneType == "寢室"
                       select o2;

            if (!string.IsNullOrEmpty(bee))
            {
                data = data.Where(o => o.ProductName.Contains(bee));
            }

            return View(await data.ToListAsync());
        }
        // 聯絡我們
        public IActionResult FooterLink01()
        {
            return View();
        }

        //常見問題
        public IActionResult FooterLink02()
        {
            return View();
        }

        //客戶意見回饋
        public IActionResult FooterLink03()
        {
            return View();
        }
        //隱私權及網站使用條款
        public IActionResult FooterLink04()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
