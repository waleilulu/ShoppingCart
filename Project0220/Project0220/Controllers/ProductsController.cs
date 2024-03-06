using Microsoft.AspNetCore.Mvc;
using Project0220.Models;

namespace Project0220.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;

        public ProductsController(ApplicationDbContext context) 
        {
            this.context = context;
        }
        public IActionResult Index()
        {   
            var products = context.Products.OrderByDescending(p=>p.ProductID).ToList();
            return View(products);
        }
    }
}
