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



namespace Project0220.Controllers
{

    public class CartController : Controller
    {
        private readonly ScaffoldEcommerceDbContext _context;

        public CartController(ScaffoldEcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            var orders = await _context.Orders.ToListAsync();
            var products = await _context.Products.ToListAsync();

            POModel data = new POModel()
            {
                Orders = orders,
                Products = products
            };

            ViewBag.Products = products;
            ViewBag.Orders = orders;



            return View(data);
        }

        //public IActionResult Create()
        //{

        //    POModel data = new POModel();
        //    ViewBag.Product = _context.Products.ToList();
        //    ViewBag.Orders = _context.Orders.ToList(); ;
        //    return View(data);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("OrderId,CustomerId,OrderDate,TotalAmount,PaymentMethod,Carrier," +
        //    "ShippingDate,ShippingAddress,Consignee,ContactPhone,ProductId,ProductName,SupplierId,CategoryId,UnitPrice,UnitInStock,Image1,Image2,Image3,Color1,Color2,Color3,Color4," +
        //    "Length,Width,Height,SpecialZoneType,CreatedAt" +
        //    "")]POModel pOModel)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        ViewData["AAA"] = pOModel;
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }


        //    return View(pOModel);
        //}



        //[HttpPost]
        //public IActionResult AddToCar(int productId, string productName)
        //{
        //    // 根据 productId 获取产品信息
        //    var product = _context.Products.FirstOrDefault(p => p.ProductId == productId && p.ProductName == productName);
        //    if (product != null)
        //    {
        //        // 将接收到的数据存储在 TempData 中
        //        TempData["SelectedProduct"] = new
        //        {
        //            ProductId = productId,
        //            ProductName = productName,

        //        };
        //    }

        //    return RedirectToAction("CheckOutDone", "Cart"); // 重定向到购物车页面
        //}


        //[HttpPost]
        //public IActionResult Index(int productId, string productName, string color1, int UnitPrice, string Image1)
        //{
        //    // 根据 productId 获取产品信息
        //    var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
        //    if (product != null)
        //    {


        // }

        // return RedirectToAction("Index", "Cart"); // 将用户重定向到购物车页面
        // }

        // 添加商品到購物車
        [HttpPost]
        public IActionResult AddToCart(int productId, int count)
        {
            List<CartItemModel> cart = GetCartFromSession(); // 使用CartItem

            var product = GetProductById(productId); // 使用productId獲取Product實例
            if (product != null)
            {
                var existingItem = cart.FirstOrDefault(item => item.Product.ProductId == productId);
                if (existingItem != null)
                {
                    // 如果購物車中已經有這個產品，則增加其數量
                    existingItem.Quantity += count;
                }
                else
                {
                    // 添加新商品
                    cart.Add(new CartItemModel { Product = product , Quantity = count });
                }

                // 更新Session
                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
            }

            return RedirectToAction("AddToCart");
        }


        // 從 Session 中獲取購物車資料
        private List<CartItemModel> GetCartFromSession()
        {
            string cartJson = HttpContext.Session.GetString("Cart");
            if (!string.IsNullOrEmpty(cartJson))
            {
                return JsonSerializer.Deserialize<List<CartItemModel>>(cartJson);
            }
            return new List<CartItemModel>();
        }

        // 根據商品ID從資料庫中獲取商品信息
        private myModels.Product GetProductById(int productId)
        {
            // 在這裡實現從資料庫中獲取商品信息的邏輯，返回一個 Product 對象
            return _context.Products.ToList().FirstOrDefault(p => p.ProductId == productId);
        }

        // 查看購物車
        public IActionResult AddToCart()
        {
            List<CartItemModel> cart = GetCartFromSession();

            // 傳遞購物車資料給視圖
            return View(cart);
        }

        //===============================================================================


        // GET: Orders/Create
        //public IActionResult Create()
        //{
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,CustomerId,OrderDate,TotalAmount,PaymentMethod,Carrier,ShippingDate,ShippingAddress,Consignee,ContactPhone")] Order order)
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


        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            List<CartItemModel> cart = GetCartFromSession(); // 從Session獲取當前購物車

            var itemToRemove = cart.FirstOrDefault(item => item.Product.ProductId == productId);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove); // 移除該商品
                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart)); // 更新Session
            }

            return RedirectToAction("AddToCart"); // 重定向到購物車視圖
        }


        public IActionResult NextConsignee()
        {

            // 檢查是否存在名為 "membercookie" 的 cookie
            if (HttpContext.Request.Cookies["membercookie"] != null)
            {
                // 如果存在相應的 cookie，繼續執行其他操作
                return RedirectToAction("Cart", "Consignee", new { id = HttpContext.Request.Cookies["membercookie"] });

            }

            // 如果用戶未通過身份驗證，導向登入頁面
            return RedirectToAction("Login", "Customers");

        }
        //====================================================================================

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

        public IActionResult CheckOutDone()
        {
            return View();
        }

        //private Project0220.myModels.Order GetOrderById(int orderid)
        //{
        //    return _context.Orders.FirstOrDefault(o => o.OrderId == orderid);
        //}

        //public IActionResult Consignee()
        //{
        //    var consigneeInfoJson = HttpContext.Session.GetString("Consignee");
        //    if (!string.IsNullOrEmpty(consigneeInfoJson))
        //    {
        //        var consigneeInfo = JsonSerializer.Deserialize<List<Project0220.myModels.Order>>(consigneeInfoJson);

        //        // 確保 consigneeInfo 不為 null 後進行後續操作
        //        if (consigneeInfo != null)
        //        {
        //            var updatedConsigneeInfoJson = JsonSerializer.Serialize(consigneeInfo);
        //            HttpContext.Session.SetString("Consignee", updatedConsigneeInfoJson);
        //        }
        //    }
        //    return View();
        //}

        //public IActionResult CheckOut()
        //{
        //    var consigneeInfoJson = HttpContext.Session.GetString("Consignee");
        //    List<Project0220.myModels.Order> consigneeInfo = null;
        //    if (!string.IsNullOrEmpty(consigneeInfoJson))
        //    {
        //        consigneeInfo = JsonSerializer.Deserialize<List<Project0220.myModels.Order>>(consigneeInfoJson);

        //        // 调试：检查反序列化的结果
        //        if (consigneeInfo == null || !consigneeInfo.Any())
        //        {
        //            // 如果这里触发，说明反序列化没有得到预期的结果
        //            Console.WriteLine("ConsigneeInfo is null or empty.");
        //        }
        //    }
        //    else
        //    {
        //        // 如果这里触发，说明Session中没有找到预期的数据
        //        Console.WriteLine("ConsigneeInfoJson is null or empty.");
        //    }

        //    return View(consigneeInfo);
        //}





    }
}


