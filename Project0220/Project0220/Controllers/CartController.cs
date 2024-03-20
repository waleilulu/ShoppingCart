using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project0220.Models;
using Project0220.myModels;
using System;
using System.Linq;


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

        public async Task<IActionResult> Index()
        {
            if (!IsAuthenticated())
            {
                // 如果身份驗證未通過，重定向到登入頁面
                return RedirectToAction("Login", "Customers");
            }

            // 獲取已登入用戶的 CustomerID
            var id = Convert.ToInt32(HttpContext.Request.Cookies["membercookie"]);

            // 從資料庫中獲取該用戶的購物車項目，同時包含相關的產品資料
            var cartItemsWithProduct = await _context.CartItems
                .Include(item => item.Product) // 包含相關的 Product 資訊
                .Where(item => item.CustomerID == id)
                .ToListAsync();

            // 將購物車項目轉換為 CartItem 模型類的實例
            var model = cartItemsWithProduct.Select(item => new CartItem
            {
                CartItemID = item.CartItemID,
                CustomerID = item.CustomerID,
                ProductID = item.ProductID,
                Quantity = item.Quantity,
                SelectedColor = item.SelectedColor,
                Product = new myModels.Product // 建立 Product 實例
                {
                    ProductId = item.Product.ProductId,
                    Image1=item.Product.Image1,
                    ProductName = item.Product.ProductName,
                    UnitPrice = item.Product.UnitPrice,
                    // 如果有其他屬性需要轉換，請繼續在此處添加
                }
            }).ToList();

            // 將購物車項目集合傳遞給視圖進行顯示
            return View(model);
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

    

