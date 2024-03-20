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

        

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("OrderId,CustomerId,OrderDate,TotalAmount,PaymentMethod,Carrier,ShippingDate,PostalCode,ShippingAddress,Consignee,ContactPhone")] Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(order);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(CheckOut));
        //    }
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
        //    return View(order);
        //}



        //public IActionResult NextConsignee()
        //{

        //    // 檢查是否存在名為 "membercookie" 的 cookie
        //    if (HttpContext.Request.Cookies["membercookie"] != null)
        //    {
        //        // 如果存在相應的 cookie，繼續執行其他操作
        //        return RedirectToAction("ChechOut", "Consignee", new { id = HttpContext.Request.Cookies["membercookie"] });

        //    }

        //    // 如果用戶未通過身份驗證，導向登入頁面
        //    return RedirectToAction("Login", "Customers");

        //}


        public IActionResult Consignee()
        {
            //var memberCookie = Convert.ToInt32(HttpContext.Request.Cookies["membercookie"]);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,CustomerId,OrderDate,TotalAmount,PaymentMethod,Carrier,ShippingDate,PostalCode,ShippingAddress,Consignee,ContactPhone")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CheckOut));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
            return View(order);
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
