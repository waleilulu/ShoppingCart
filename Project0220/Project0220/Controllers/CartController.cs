using Microsoft.AspNetCore.Mvc;
using Project0220.Models;
using Project0220.myModels;
using System;


namespace Project0220.Controllers
{
    public class CartController : Controller
    {
        private readonly ScaffoldEcommerceDbContext _context;

        public CartController(ScaffoldEcommerceDbContext context)
        {
            _context = context;
        }

        private bool IsAuthenticated()
        {
            // 檢查是否存在名為 "membercookie" 的 cookie
            var memberCookie = HttpContext.Request.Cookies["membercookie"];
            return !string.IsNullOrEmpty(memberCookie);
        }

        public IActionResult Index()
        {
            if (IsAuthenticated())
            {
                // 身份驗證通過，執行相應的操作
                // 從資料庫中獲取購物車數據

                // 獲取已登入用戶的CustomerID
                var memberCookie = HttpContext.Request.Cookies["membercookie"];
                var customerID = _context.Customers
        .FirstOrDefault(c => c.CustomerId.ToString() == memberCookie)?.CustomerId;

                if (customerID != null)
                {
                    // 從資料庫中獲取該用戶的購物車項目
                    var cartItems = _context.CartItems
                        .Where(item => item.CustomerID == customerID)
                        .ToList();

                    // 將購物車項目轉換為 CartItem 模型類的實例
                    var model = cartItems.Select(item => new CartItem
                    {
                        CustomerID = item.CustomerID,
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        SelectedColor = item.SelectedColor,
                    }).ToList();

                    // 將購物車項目集合傳遞給視圖進行顯示
                    return View(model);
                }
                else
                {
                    // 如果找不到對應的CustomerID，可能需要進一步處理
                    // 此處示例中將重定向到登入頁面
                    return RedirectToAction("Login", "Customers");
                }
            }
            else
            {
                // 身份驗證未通過，重定向到登入頁面
                return RedirectToAction("Login", "Customers");
            }
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity, string selectedcolor)
        {
            // 檢查用戶是否已通過身份驗證，以確保只有登入用戶才能添加到購物車
            if (IsAuthenticated())
            {
                // 獲取已登入用戶的CustomerID
                var memberCookie = HttpContext.Request.Cookies["membercookie"];
                var customerID = _context.Customers
                    .FirstOrDefault(c => c.CustomerId.ToString() == memberCookie)?.CustomerId;

                if (customerID != null)
                {
                    // 創建新的 CartItem 實例，並設置相關屬性
                    var newCartItem = new CartItem
                    {
                        //CustomerID = customerID,
                        CustomerID = customerID.HasValue ? customerID.Value : default(int), // 如果 customerID 有值，則取其值；否則設置為 int 的默認值
                        ProductID = productId,
                        Quantity = quantity,
                        SelectedColor = selectedcolor // 將選擇的顏色設置到新的 CartItem 中
                    };

                    // 將新的 CartItem 添加到資料庫
                    _context.CartItems.Add(newCartItem);
                    _context.SaveChanges();

                    // 返回成功的消息或重定向到購物車頁面
                    return RedirectToAction("Index");
                }
                else
                {
                    // 如果找不到對應的CustomerID，可能需要進一步處理
                    // 此處示例中將重定向到登入頁面
                    return RedirectToAction("Login", "Customers");
                }
            }
            else
            {
                // 如果未通過身份驗證，可能需要進一步處理
                // 此處示例中將重定向到登入頁面
                return RedirectToAction("Login", "Customers");
            }
        }


    }

}

    

