using Microsoft.AspNetCore.Mvc;

namespace Project0220.Controllers
{
    public class MemberController : Controller
    {   //系統預設
        public IActionResult Index()
        {
            return View();
        }
        //會員中心
        public IActionResult MemberHome()
        {
            return View();
        }
        //準備作廢
        public IActionResult Member()
        {
            return View();
        }
        [HttpPost]
        public string Member(int id, string Name, string Birthday, string Gender, int Phone, string email, string city, string district, string address, string username, string password)
        {
            return $"OK-Post-{id}-{Name}-{Birthday}-{Gender}-{Phone}-{email}-{city}-{district}-{address}-{username}-{password}";
        }
        //會員登入
        public IActionResult MemberLogin()
        {
            return View();
        }

        //會員註冊
        public IActionResult MemberRegister()
        {
            return View();
        }
    }
}
