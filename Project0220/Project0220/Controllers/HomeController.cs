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

        // �p���ڭ�
        public IActionResult FooterLink01()
        {
            return View();
        }

        //�`�����D
        public IActionResult FooterLink02()
        {
            return View();
        }

        //�Ȥ�N���^�X
        public IActionResult FooterLink03()
        {
            return View();
        }
        //���p�v�κ����ϥα���
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
