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

        private bool IsAuthenticated()
        {
            // 檢查是否存在名為 "membercookie" 的 cookie
            var memberCookie = HttpContext.Request.Cookies["membercookie"];
            return !string.IsNullOrEmpty(memberCookie);
        }

        public IActionResult Consignee()
        {
            //var memberCookie = Convert.ToInt32(HttpContext.Request.Cookies["membercookie"]);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            return View();

        }

        //[HttpPost]
        //public IActionResult Create(int customerId, int orderId, string consignee, 
        //    string contactPhone, string Carrier, string ShippingDate, string PostalCode, 
        //    string ShippingAddress, string PaymentMethod
        //    )
        //{  
        //    // 從Cookies獲取已登入用戶的CustomerID
        //    var memberCookie = HttpContext.Request.Cookies["membercookie"];
        //    var customerIDFromCookie = _context.Customers
        //        .FirstOrDefault(c => c.CustomerId.ToString() == memberCookie)?.CustomerId;

        //if (customerIDFromCookie == null || customerIDFromCookie != customerId)
        //{
        //    // 如果cookie中的customerID與傳入的customerID不匹配，返回錯誤
        //    return Unauthorized();
        //}
        //// 檢查用戶是否已通過身份驗證，以確保只有登入用戶才能添加收件人資料
        //if (!IsAuthenticated())
        //{
        //    // 如果用戶未登入，可以選擇重定向到登入頁面或返回錯誤
        //    return RedirectToAction("Login", "Customer"); // 假設有一個名為Login的Action
        //}



        //根據customerId和orderId找到對應的Order
        //var order = _context.Orders.FirstOrDefault(o => o.CustomerId == customerId && o.OrderId == orderId);
        // if (order == null)
        // {
        //     // 如果沒有找到訂單，返回錯誤
        //     return NotFound();
        // }

        // // 更新訂單的收件人資料
        // order.Consignee = consignee;
        // order.ContactPhone = contactPhone;
        // order.Carrier = Carrier;
        // order.ShippingAddress = ShippingAddress;
        // order.PostalCode = PostalCode;
        // order.PaymentMethod = PaymentMethod;

        //if (DateOnly.TryParse(ShippingDate, out DateOnly parsedDate))
        //{
        //    order.ShippingDate = parsedDate;
        //}
        //else
        //{
        //    // 日期轉換失敗，向ModelState添加一個錯誤
        //    ModelState.AddModelError("ShippingDate", "日期轉換失敗。請確保日期格式正確，例如：2023-01-31。");

        //    // 可以選擇將用戶重新導向到表單頁面，並顯示錯誤消息
        //    // 假設您有一個名為"EditOrder"的視圖，用於編輯訂單
        //    return View("CheckOut", order); // 確保將模型傳回視圖，以便用戶可以看到他們原先的輸入和錯誤消息
        //}


        // 保存變更到資料庫
        // _context.SaveChanges();

        // 返回適當的響應，比如重定向到某個頁面或返回成功訊息
        //    return RedirectToAction("CheckOut", new { orderId = orderId }); // 有一個顯示訂單詳情的Action
        //}


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

        //// GET: Orders/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var order = await _context.Orders.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
        //    return View(order);
        //}


        public async Task<IActionResult> CheckOut()
        {
            var lastOrder = await _context.Orders.OrderByDescending(o => o.OrderId).FirstOrDefaultAsync();

            // 檢查是否有訂單存在
            if (lastOrder == null)
            {
                // 處理沒有訂單的情況，例如顯示錯誤信息或重定向
                return View("ErrorView"); // 假設有一個顯示錯誤的視圖
            }

            // 如果視圖需要處理多個訂單，保留下列代碼
            return View(new List<Order> { lastOrder });

            // 如果視圖只需要一個訂單，使用這行代碼
            //return View(lastOrder);
        }

            

        

        public async Task<IActionResult> CheckOutDone()
        {
            var lastOrder = await _context.Orders.OrderByDescending(o => o.OrderId).FirstOrDefaultAsync(); //遞減排序的第一個

            return View(new List<Order> { lastOrder });

        }
    }
}
