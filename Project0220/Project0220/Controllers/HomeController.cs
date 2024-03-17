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

            return View(data);
        }
        

        public IActionResult Privacy()
		{
			return View();
		}
        public IActionResult NewHome()
        {
            return View();
        }
        public IActionResult Family()
        {
            return View();
        }
        public IActionResult Bedroom()
        {
            return View();
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
