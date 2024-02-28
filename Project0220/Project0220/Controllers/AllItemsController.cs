using Microsoft.AspNetCore.Mvc;

namespace Project0220.Controllers
{
    public class AllItemsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
