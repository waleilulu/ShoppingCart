using Microsoft.AspNetCore.Mvc;
using Project0220.Models;
using System;


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

        public IActionResult CheckOutDone()
        {
            return View();
        }

        public IActionResult PayMent()
        {
            return View();
        }

    }
}
    

