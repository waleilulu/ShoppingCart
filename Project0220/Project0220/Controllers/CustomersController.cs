using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Project0220.myModels;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Configuration;
using Project0220.ViewModel;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Drawing;

namespace Project0220.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ScaffoldEcommerceDbContext _context;
        private readonly IConfiguration _configuration;

        public CustomersController(ScaffoldEcommerceDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }


        // GET: Customers/Details/5
        public async Task<IActionResult> Details()
        {
            var id = Convert.ToInt32(HttpContext.Request.Cookies["membercookie"]);
            // 根據 Session 中的用戶ID查找用戶
            var user = await _context.Customers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            //    var customer = await _context.Customers
            //      .FirstOrDefaultAsync(m => m.CustomerId == id);

            // 從 TrackList 中找出該用戶追蹤的所有產品ID
            var trackedProductIds = await _context.TrackLists
                .Where(tl => tl.CustomerID == id)
                .Select(tl => tl.ProductID)
                .ToListAsync();

            //根據這些產品ID查找相應的產品資訊
            var trackedProducts = await _context.Products
             .Where(p => trackedProductIds.Contains(p.ProductId))
              .ToListAsync();

            var orders = await _context.Orders
       .Where(o => o.CustomerId == id)
       .OrderByDescending(o => o.OrderDate) // 按訂單日期降序排序
       .ToListAsync();

            var orderDetails = await _context.OrderDetails
                .Where(od => orders.Select(o => o.OrderId).Contains(od.OrderId.Value))
                .ToListAsync();

            // 填充 OrdersWithDetails
            var ordersWithDetails = orders.Select(order => new OrderWithDetails
            {
                Order = order,
                OrderDetails = orderDetails.Where(od => od.OrderId == order.OrderId).ToList()
            }).ToList();

            // Products 包含所有的商品信息
            var allProducts = await _context.Products.ToListAsync();

            var viewModel = new ViewModel.CPTModel
            {
                Customers = new List<Customer> { user },
                Products = allProducts,
                TrackedProducts = trackedProducts, // 追踪商品單獨儲存
                OrdersWithDetails = ordersWithDetails,

            };

            return View(viewModel);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,CustomerName,DateOfBirth,Gender,MobilePhoneNumber,Email,AddressCity,AddressDist,Address,Username,Password,Subscribe")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                //檢查用戶名是否存在
                var existingUser = await _context.Customers.FirstOrDefaultAsync(c => c.Username == customer.Username);
				if (existingUser != null)
				{
					ModelState.AddModelError("Username", "用戶名已被註冊，請選擇另一個用戶名");
					return View(customer);
				}

				// 計算用户年齡
				int age = DateTime.Today.Year - customer.DateOfBirth?.Year ?? 0;
				if (customer.DateOfBirth?.Date > DateTime.Today.AddYears(-age)) age--;

				// 檢查用戶年齡是否小於18
				if (age < 18)
				{
					ModelState.AddModelError("DateOfBirth", "您必須年滿18歲才能註冊。");
					return View(customer);
				}
				if (string.IsNullOrEmpty(customer.Password) || customer.Password.Length < 6)
				{
					ModelState.AddModelError("Password", "密碼是必填的，且長度必須大於6個字。");
					return View(customer);
				}


				// 对用户密码进行加密
				string hashedPassword = BCrypt.Net.BCrypt.HashPassword(customer.Password);

                // 将加密后的密码赋值给用户对象
                customer.Password = hashedPassword;

                customer.Subscribe = HttpContext.Request.Form["subscribe"] == "on" ? true : false;
               
                _context.Add(customer);
                await _context.SaveChangesAsync();
                // 設置註冊成功的提示消息
                TempData["RegisterSuccessMessage"] = "您已成功註冊會員！";
               
                return RedirectToAction("Login", "Customers");
            }
            return View(customer);
        }
        //會員登入
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        // 登入動作
        public async Task<IActionResult> Login([Bind("Username,Password")] Customer Customers)
        {
            // 查询数据库以查找用户
            var user = _context.Customers.SingleOrDefault(u => u.Username == Customers.Username);

            if (user != null)
            {
                // 验证用户输入的密码与数据库中存储的哈希密码是否匹配
                if (BCrypt.Net.BCrypt.Verify(Customers.Password, user.Password))
                {
                    // 创建用户主张
                    var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Name, user.Username)
                // 可以添加其他用户相关的主张，例如用户角色等
                };

                    if (user.Admin)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
                        HttpContext.Response.Cookies.Append("userRole", "Administrator");

                
                    }

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // 创建身份验证票证
                    var principal = new ClaimsPrincipal(identity);

                    // 登录用户
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    // 设置 Cookie 保存用户 ID
                    HttpContext.Response.Cookies.Append("membercookie", user.CustomerId.ToString());
                    HttpContext.Session.SetInt32("userId", (int)user.CustomerId);
                    // 根据用户角色重定向到适当的页面
                    if (user.Admin)
                    {
                        return RedirectToAction("Admin", "Customers");
                    }
                    else
                    {
                        return RedirectToAction("Details", "Customers", new { id = user.CustomerId });
                    }
                }
            }

            // 登录失败，返回登录视图并显示错误消息
            ModelState.AddModelError("", "登入失敗，請檢查用户名和密碼。");
            return View("Login");
        }
        //管理者選擇頁面

        public IActionResult Admin() {
          
            return View();
		}


        //登出
        //登出動作方法
        [HttpPost]
        public IActionResult Logout()
        {
            // 執行登出操作
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // 清除用戶相關的 Session 和 Cookie
            HttpContext.Session.Clear();
            Response.Cookies.Delete("membercookie");
			Response.Cookies.Delete("userRole");
			Response.Cookies.Delete("isAdmin");
			// 重定向到登入頁面
			return RedirectToAction("Login", "Customers");
        }

        //決定導向哪裡的方法( 會員頁面 還是 登入頁面 )


        public IActionResult UserProfile()
        {
            // 检查是否存在名为 "membercookie" 的 cookie
            if (HttpContext.Request.Cookies["membercookie"] != null)
            {
                var isAdmin = HttpContext.Request.Cookies["isAdmin"];
                var userRole = HttpContext.Request.Cookies["userRole"];
                if ( isAdmin == "true")
                {
                    return RedirectToAction("Details", "Customers", new { id = HttpContext.Request.Cookies["membercookie"] });
                }
                // 如果用户角色為管理員，則重定向到管理員页面
               else if (userRole =="Administrator")
				{
					return RedirectToAction("Admin", "Customers");
				}
				// 如果用户角色为会员，则重定向到会员详情页面
				else
                {
                    return RedirectToAction("Details", "Customers", new { id = HttpContext.Request.Cookies["membercookie"] });
                }
            }

            // 如果用户未通过身份验证，则重定向到登录页面
            return RedirectToAction("Login", "Customers");

        }




        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var Customer = await _context.Customers.FindAsync(id);
            if (Customer == null)
            {
                return NotFound();
            }
            return View(Customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,CustomerName,DateOfBirth,Gender,MobilePhoneNumber,Email,AddressCity,AddressDist,Address,Username,Password")] Customer Customers)
        {
            if (id != Customers.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
					// 查找現有客戶記錄
					var existingCustomer = await _context.Customers.FindAsync(id);

					// 將現有客戶記錄的密碼賦值給編輯後的模型
					Customers.Password = existingCustomer.Password;

					// 更新客戶記錄，但不包括密碼
					_context.Entry(existingCustomer).CurrentValues.SetValues(Customers);

					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(Customers.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Customers", new { id = HttpContext.Request.Cookies["membercookie"] });

            }
            return View(Customers); //模型狀態無效，将用户保留在编辑页面，并显示验证错误
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }

        //刪除追蹤商品
        [HttpPost]
        public IActionResult DeleteProduct(int productId)
        {

            var CustomerId = Convert.ToInt32(HttpContext.Request.Cookies["membercookie"]);
            var trackList = _context.TrackLists.FirstOrDefault(t => t.CustomerID == CustomerId && t.ProductID == productId  );


            if (trackList != null)
            {
                _context.TrackLists.Remove(trackList);
                _context.SaveChanges();


                return Json(new { success = true, message = "追蹤商品刪除成功" }); // 返回刪除成功的視圖
            }
            return Json(new { success = true, message = "追蹤商品刪除失敗" });
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }
        private string GenerateVerificationCode()
        {
            // 實現生成驗證碼的邏輯，例如使用 Guid 或隨機數字生成
            // 這裡僅為示例，您可以根據需要自定義生成驗證碼的方法
            return Guid.NewGuid().ToString("N").Substring(0, 6);
        }

        //忘記密碼
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(Customer Model)
        {
                // 檢查輸入的用戶名和郵箱是否匹配
                var user = await _context.Customers.FirstOrDefaultAsync(c => c.Username == Model.Username && c.Email == Model.Email);

                if (user != null)
                {
                    var verificationCode = GenerateVerificationCode();
                    user.ResetPasswordToken = verificationCode;
                    user.ResetPasswordTokenExpiration = DateTime.Now.AddMinutes(1); // 驗證碼有效期1分鐘
                    await _context.SaveChangesAsync();

                    // 發送驗證碼到用戶提供的 email 中
                    await SendEmails(user.Email, verificationCode);
 
                return RedirectToAction("ForgetPassword");
               
            }
            // 如果用戶名和郵箱不匹配，返回忘記密碼頁面並顯示錯誤消息
            ModelState.AddModelError(string.Empty, "提供的用戶名和郵箱不匹配。");
            TempData["Message"] = "提供的用戶名和郵箱不匹配。";
            return View();
        }

        private async Task SendEmails(string email, string verificationCode)
        {
            // 使用 Google Mail Server 發信
            string account = _configuration["EmailSettings:Account"];
            string password = _configuration["EmailSettings:Password"];

            string SmtpServer = "smtp.gmail.com";
            int SmtpPort = 587;
            MailMessage mms = new MailMessage();
            mms.From = new MailAddress(account);
            mms.Subject = "信件主題";

            mms.To.Add(new MailAddress(email)); // 添加收件人

            mms.Body = $@"<html>
                    <head>
                        <title>驗證密碼</title>
                    </head>
                    <body>
                        <p>親愛的用戶，您的驗證碼為{verificationCode}，驗證碼期限為1分鐘，請在期限內完成驗證。</p>
                    </body>
                </html>";
            mms.IsBodyHtml = true;
            mms.SubjectEncoding = Encoding.UTF8;

            using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(account, password);//寄信帳密 
                client.Send(mms); //寄出信件
            }

        }

        [HttpPost]
        public async Task<IActionResult> EnterVerificationCode(string verificationCode)
        {
            if (ModelState.IsValid)
            {
                // 查找用戶
                var user = await _context.Customers.FirstOrDefaultAsync(c => c.ResetPasswordToken == verificationCode);
                if (user != null && user.ResetPasswordTokenExpiration > DateTime.Now)
                {
                    // 清除重置密碼
                    user.ResetPasswordToken = "";
                    user.ResetPasswordTokenExpiration = null;
                    await _context.SaveChangesAsync();

                    // 將用戶重定向到重設密碼頁面
                    return RedirectToAction("ResetPWD", "ResetPwd");
                }
                else if (user != null && user.ResetPasswordTokenExpiration <= DateTime.Now)
                {
                    // 驗證碼已過期
                    ModelState.AddModelError(string.Empty, "驗證碼已過期，請重新取得驗證碼。");
                    TempData["ErrorMessage"] = "驗證碼已過期，請重新取得驗證碼。";
                    return RedirectToAction("ForgetPassword");
                }
            }

            // 如果驗證碼不正確，或用戶不存在，返回輸入驗證碼的頁面並顯示錯誤消息
            ModelState.AddModelError(string.Empty, "驗證碼錯誤，請重新輸入。");
            TempData["ErrorMessage"] = "驗證碼錯誤，請重新輸入。";
            return RedirectToAction("ForgetPassword");
        }

        public IActionResult Create_Admin()
		{
			return View();
		}


		// GET: Customers/Edit/5
		public async Task<IActionResult> Edit_Admin(int? id)
		{

			if (id == null)
			{
				return NotFound();
			}

			var Customer = await _context.Customers.FindAsync(id);
			if (Customer == null)
			{
				return NotFound();
			}
			return View(Customer);
		}

		// POST: Customers/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit_Admin(int id, [Bind("CustomerId,CustomerName,DateOfBirth,Gender,MobilePhoneNumber,Email,AddressCity,AddressDist,Address,Username,Password")] Customer Customers)
		{
			if (id != Customers.CustomerId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(Customers);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CustomerExists(Customers.CustomerId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction("Details", "Customers", new { id = HttpContext.Request.Cookies["membercookie"] });

			}
			return View(Customers); //模型狀態無效，将用户保留在编辑页面，并显示验证错误
		}




	}

}





