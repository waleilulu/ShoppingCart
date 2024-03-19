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

namespace Project0220.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ScaffoldEcommerceDbContext _context;

        public CustomersController(ScaffoldEcommerceDbContext context)
        {
            _context = context;
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
            // 根據這些產品ID查找相應的產品資訊
            var trackedProducts = await _context.Products
                .Where(p => trackedProductIds.Contains(p.ProductId))
                .ToListAsync();

            var viewModel = new ViewModel.CPTModel
            {
                Customers = new List<Customer> { user },
                Products = trackedProducts

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
                customer.Subscribe = HttpContext.Request.Form["subscribe"] == "on" ? true : false;
                // 添加 Customer 到数据库中
                _context.Add(customer);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Login));
            }
            return View(customer);
        }

        //會員登入
        public IActionResult Login()
        {
            return View();
        }

        // 登入動作
        [HttpPost]
        public async Task<IActionResult> Login([Bind("Username,Password")] Customer Customers)
        {
            // 查询数据库以查找用户并进行身份验证
            var user = _context.Customers.SingleOrDefault(u => u.Username == Customers.Username && u.Password == Customers.Password);

            if (user != null)
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

            // 重定向到登入頁面
            return RedirectToAction("Login", "Customers");
        }

        //決定導向哪裡的方法( 會員頁面 還是 登入頁面 )


        public IActionResult UserProfile()
        {
            
                // 檢查是否存在名為 "membercookie" 的 cookie
                if (HttpContext.Request.Cookies["membercookie"] != null)
                {
                    // 如果存在相應的 cookie，繼續執行其他操作
                    // 這裡可以放置會員中心頁面的相關代碼
                    return RedirectToAction("Details", "Customers", new { id = HttpContext.Request.Cookies["membercookie"] });


                }

                    // 如果用戶未通過身份驗證，導向登入頁面
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
        
        
    }
}
