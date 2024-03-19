using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Project0220.Models;
using Project0220.myModels;
using Project0220.ViewModel;
using System.Text.Json;
using System;
using System.Drawing;
using System.Linq;
using static NuGet.Packaging.PackagingConstants;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics.Metrics;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Xml.Linq;
namespace Project0220.Controllers
{
    public class CheckOutController : Controller { 

   private readonly ScaffoldEcommerceDbContext _context;

    public CheckOutController(ScaffoldEcommerceDbContext context)
    {
        _context = context;
    }
    

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Consignee()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            return View();

        }

        public async Task<IActionResult> CheckOut()
        {
            var lastOrder = await _context.Orders.OrderByDescending(o => o.OrderId).FirstOrDefaultAsync(); //遞減排序的第一個

            return View(new List<Order> { lastOrder });

        }

        public async Task<IActionResult> CheckOutDone()
        {
            var lastOrder = await _context.Orders.OrderByDescending(o => o.OrderId).FirstOrDefaultAsync(); //遞減排序的第一個

            return View(new List<Order> { lastOrder });

        }
    }
}
