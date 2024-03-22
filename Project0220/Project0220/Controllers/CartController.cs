using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project0220.Models;
using Project0220.myModels;
using System;
using System.Linq;
using System.Text.Json.Serialization;


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
                Product = new myModels.Product // 建立 Product 實例，從另外一張資料表撈資料
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
					// 找到現有的 CartItem，包括顏色的比較。如果商品有多個顏色，加入不同顏色商品也會視為不同項目
					var existingCartItem = _context.CartItems
						.FirstOrDefault(c => c.CustomerID == customerID && c.ProductID == productId && c.SelectedColor == selectedcolor);

					if (existingCartItem != null)
					{
						// 更新現有的 CartItem 的數量
						existingCartItem.Quantity += quantity;
						_context.SaveChanges();
					}
					else
					{
						// 創建新的 CartItem 實例，並設置相關屬性
						var newCartItem = new CartItem
						{
							CustomerID = customerID.HasValue ? customerID.Value : default(int),
							ProductID = productId,
							Quantity = quantity,
							SelectedColor = selectedcolor // 將選擇的顏色設置到新的 CartItem 中
						};

						// 將新的 CartItem 添加到資料庫
						_context.CartItems.Add(newCartItem);
						_context.SaveChanges();
					}

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

		[HttpPost]
		public IActionResult RemoveFromCart(int cartItemId)
		{
			// 檢查用戶是否已通過身份驗證
			if (IsAuthenticated())
			{
				// 獲取已登入用戶的 CustomerID
				var memberCookie = HttpContext.Request.Cookies["membercookie"];
				var customerID = _context.Customers.FirstOrDefault(c => c.CustomerId.ToString() == memberCookie)?.CustomerId;

				if (customerID != null)
				{
					// 根據 cartItemId 從購物車中找到對應的購物車項目
					var cartItemToRemove = _context.CartItems.FirstOrDefault(ci => ci.CartItemID == cartItemId && ci.CustomerID == customerID);

					if (cartItemToRemove != null)
					{
						// 從資料庫中移除該購物車項目
						_context.CartItems.Remove(cartItemToRemove);
						_context.SaveChanges();

						// 返回成功的消息或重定向到購物車頁面
						return RedirectToAction("Index");
					}
				}
				else
				{
					// 如果找不到對應的 CustomerID，可能需要進一步處理
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

			// 如果找不到對應的購物車項目，可能需要進一步處理
			// 此處示例中將返回一個錯誤消息或重定向到購物車頁面
			return RedirectToAction("Index");
		}
        [HttpPost]
        public IActionResult UpdateCartItemQuantity(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                CartItem cartItem = JsonConvert.DeserializeObject<CartItem>(data);
                // 檢查使用者是否已通過身份驗證
                if (IsAuthenticated())
                {
                    // 獲取已登入使用者的 CustomerID
                    var memberCookie = HttpContext.Request.Cookies["membercookie"];
                    var customerID = _context.Customers.FirstOrDefault(c => c.CustomerId.ToString() == memberCookie)?.CustomerId;

                    if (customerID != null)
                    {
                        // 根據 cartItemId 找到對應的購物車項目
                        var cartItemToUpdate = _context.CartItems.FirstOrDefault(ci => ci.CartItemID == cartItem.CartItemID && ci.CustomerID == customerID);

                        if (cartItemToUpdate != null)
                        {
                            // 更新購物車項目的數量為新數量
                            cartItemToUpdate.Quantity = cartItem.Quantity;

                            // 將更改保存到資料庫
                            _context.SaveChanges();

                            // 返回成功的訊息或重定向到購物車頁面
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        // 如果找不到對應的 CustomerID，可能需要進一步處理
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

                // 如果找不到對應的購物車項目，可能需要進一步處理
                // 此處示例中將返回一個錯誤訊息或重定向到購物車頁面
                return RedirectToAction("Index");
            }
            else
            {
                return Json(new { success = false, message = "資料為空,請檢查資料!" });

            }
        }
        [HttpPost]
        public IActionResult EmptyCart()
        {
            if (IsAuthenticated())
            {
                // 獲取已登入用戶的CustomerID
                var memberCookie = HttpContext.Request.Cookies["membercookie"];
                var customerID = _context.Customers.FirstOrDefault(c => c.CustomerId.ToString() == memberCookie)?.CustomerId;

                if (customerID != null)
                {
                    // 找到該用戶的所有購物車項目
                    var cartItems = _context.CartItems.Where(ci => ci.CustomerID == customerID);

                    if (cartItems.Any())
                    {
                        // 移除所有項目
                        _context.CartItems.RemoveRange(cartItems);
                        _context.SaveChanges();

                        // 可以選擇返回一個成功的訊息，或是重定向到購物車頁面等
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // 如果購物車已經是空的，可以選擇返回一個訊息或進行其他處理
                        return Json(new { success = false, message = "購物車已經是空的。" });
                    }
                }
                else
                {
                    // 如果找不到對應的CustomerID，可能需要進一步處理
                    return RedirectToAction("Login", "Customers");
                }
            }
            else
            {
                // 如果未通過身份驗證，重定向到登入頁面
                return RedirectToAction("Login", "Customers");
            }
        }

    }

}

    

