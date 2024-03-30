using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project0220.myModels;

namespace Project0220.Controllers
{
    public class UpdatePwdController : Controller
    {
        private readonly ScaffoldEcommerceDbContext _context;

        public UpdatePwdController(ScaffoldEcommerceDbContext context)
        {
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult UpdatePassword() { 
        
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(string password, string newPwd, string newPwd2)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(newPwd) || string.IsNullOrEmpty(newPwd2))
            {
                ModelState.AddModelError("", "請填寫所有欄位。");
                TempData["ErrorMessage"] = "請填寫所有欄位。";
                return View();
            }

            var customers = await _context.Customers.ToListAsync(); // 獲取所有顧客資訊
            var user = customers.FirstOrDefault(c => BCrypt.Net.BCrypt.Verify(password, c.Password));

            if (user != null)
            {
                if (newPwd == newPwd2)
                {
                    var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
                    var hashPassword = BCrypt.Net.BCrypt.HashPassword(newPwd, salt);

                    try
                    {
                        user.Password = hashPassword;
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        ModelState.AddModelError("", "無法保存變更，密碼已被註冊，請輸入其他密碼。");
                        TempData["ErrorMessage"] = "無法保存變更，密碼已被註冊，請輸入其他密碼。";
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("newPwd2", "兩次輸入的密碼不一致");
                    TempData["ErrorMessage"] = "兩次輸入的密碼不一致。";
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "舊密碼輸入錯誤。");
                TempData["ErrorMessage"] = "舊密碼輸入錯誤。";
                return View();
            }
        }



    }
}
