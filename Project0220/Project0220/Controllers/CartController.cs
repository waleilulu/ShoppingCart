using Microsoft.AspNetCore.Mvc;

namespace Project0220.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CheckOut()
        {
            return View();
        }
    }
}
