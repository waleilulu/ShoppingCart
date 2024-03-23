using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project0220.myModels;

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

            /*user.Password = HashPassword(newPwd); */// 这里假设你有一个加密密码的方法 HashPassword
            if (user != null && newPwd == newPwd2)
            {
                // 更新密码
                /*user.Password = HashPassword(newPwd);*/ // 这里假设你有一个加密密码的方法 HashPassword

                try
                {
                    user.Password = newPwd2; // 设置新密码
                    _context.Update(user); 
                                await _context.SaveChangesAsync();
                    return RedirectToAction("Login", "Customers");
                }
                catch (DbUpdateConcurrencyException)
                {
                    // 处理并发更新异常
                    ModelState.AddModelError("", "無法保存變更，密碼已被註冊，請輸入其他密碼。");
                    return View();
                }
            }
            else if (user != null && newPwd != newPwd2)
            {
                ModelState.AddModelError("newPwd2", "兩次輸入的密碼不一致");
                return View();
            }
            else
            {
                return NotFound();
            }           
        }

        //private string HashPassword(string password)
        //{
        //    // 这里可以使用你选择的密码哈希算法来对密码进行加密
        //    // 例如： return SomeHashingAlgorithm.ComputeHash(password);
        //    // 请确保在此过程中使用了适当的加盐和哈希强度。
        //    return password;
        //}

    }
}
