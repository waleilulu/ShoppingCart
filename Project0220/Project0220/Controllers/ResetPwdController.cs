using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project0220.myModels;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;

namespace Project0220.Controllers
{

    public class ResetPwdController : Controller
    {
        private readonly ScaffoldEcommerceDbContext _context;

        public ResetPwdController(ScaffoldEcommerceDbContext context)
        {
            _context = context;
            
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResetPWD()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPWD(string newPwd, string newPwd2, string Username)
        {
            var user = await _context.Customers.FirstOrDefaultAsync(c => c.Username == Username);

            
            if (user != null && newPwd == newPwd2)
            {
                var salt = BCrypt.Net.BCrypt.GenerateSalt(10);

                // 哈希密码
                var hashPassword = BCrypt.Net.BCrypt.HashPassword(newPwd, salt);
                Console.WriteLine("Hashed Password: " + hashPassword);

                try
                {
                    user.Password = hashPassword; // 设置新密码为哈希后的密码
                    _context.Update(user); 
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login", "Customers");
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                    ModelState.AddModelError("", "無法保存變更，密碼已被註冊，請輸入其他密碼。");
                    TempData["ErrorMessage"] = "無法保存變更，密碼已被註冊，請輸入其他密碼。";
                    return View();
                }
            }
            else if (user != null && newPwd != newPwd2)
            {
                ModelState.AddModelError("newPwd2", "兩次輸入的密碼不一致");
                TempData["ErrorMessage"] = "兩次輸入的密碼不一致。";
                return View();
            }
            else
            {
                return NotFound();
            }           
        }
    }
}
