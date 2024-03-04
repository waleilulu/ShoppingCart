using Microsoft.AspNetCore.Mvc;
using Project0220.Models;
using System.Diagnostics;

namespace Project0220.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
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
