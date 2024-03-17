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
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;


namespace Project0220.Controllers
{
    public class EmailController : Controller
    {
        private readonly ScaffoldEcommerceDbContext _context;

        public EmailController(ScaffoldEcommerceDbContext context)
        {
            _context = context;
        }


        // 發送郵件的方法
        public async Task SendAdEmail(string emailAddress)
        {
            var subscribedEmails = await _context.Customers
            .Where(c => c.Subscribe)
            .Select(c => c.Email)
            .ToListAsync();

            // 使用 Google Mail Server 發信
            string GoogleID = "a0983984816@gmail.com"; //Google 發信帳號
            string TempPwd = "ptfs mdtv ohtu sgsk"; //應用程式密碼
            string ReceiveMail = string.Join(",", subscribedEmails);

            string SmtpServer = "smtp.gmail.com";
            int SmtpPort = 587;
            MailMessage mms = new MailMessage();
            mms.From = new MailAddress(GoogleID);
            mms.Subject = "信件主題";
            string[] mailList = ReceiveMail.Split(",");
            foreach (string mail in mailList)
            {
                mms.To.Add(new MailAddress(mail));

            }
            mms.Body = @"<html>
                    <head>
                        <title>會員優惠</title>
                    </head>
                    <body>
                        <h1>最新優惠來了！</h1>
                        <p>親愛的用戶，我們為您帶來了最新的優惠活動：凡是消費滿$5000，即可獲得$500元現金折價券。</p>
                        <p>立即訪問我們的網站以了解更多詳情： <a href=""https://example.com"">https://example.com</a></p>
                        <p>祝您購物愉快！</p>
                    </body>
                </html>"; ;
            mms.IsBodyHtml = true;
            mms.SubjectEncoding = Encoding.UTF8;
            
            using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(GoogleID, TempPwd);//寄信帳密 
                client.Send(mms); //寄出信件
            }

        }
        //    // 發送廣告郵件給所有訂閱客戶
        //    public async Task<IActionResult> SendAdEmailToSubscribedCustomers()
        //{
        //    var subscribedCustomers = await _context.Customers.Where(c => c.Subscribe).ToListAsync();
        //    // 從資料庫中檢索所有訂閱了的客戶
        //    foreach (var customer in subscribedCustomers)
        //    {
        //        // 如果客戶訂閱了，則發送廣告郵件
        //        await SendAdEmail(customer.Email); // 傳遞郵件地址給 SendAdEmail 方法
        //    }

        //    return Json(new { success = true, message = "廣告郵件已發送給所有訂閱客戶" });
        //}

    }

}
