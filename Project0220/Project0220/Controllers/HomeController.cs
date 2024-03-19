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
        public async Task<IActionResult> NewHome(string bee)
        {

            var data = from o2 in _contextNew.Products
                       where o2.SpecialZoneType == "�s�a"
                       select o2;

            if (!string.IsNullOrEmpty(bee))
            {
                data = data.Where(o => o.ProductName.Contains(bee));
            }

            return View(await data.ToListAsync());
        }


        public async Task<IActionResult> Family(string bee)
        {

            var data = from o2 in _contextNew.Products
                       where o2.SpecialZoneType == "�p�a�x"
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
                       where o2.SpecialZoneType == "���"
                       select o2;

            if (!string.IsNullOrEmpty(bee))
            {
                data = data.Where(o => o.ProductName.Contains(bee));
            }

            return View(await data.ToListAsync());
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
