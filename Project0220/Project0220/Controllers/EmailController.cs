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
using Microsoft.AspNetCore.DataProtection;



namespace Project0220.Controllers
{
    public class EmailController : Controller
    {
        private readonly ScaffoldEcommerceDbContext _context;
        private readonly IConfiguration _configuration;

        public EmailController(ScaffoldEcommerceDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // 發送郵件的方法
        public async Task SendAdEmail(string emailAddress)
        {
            var subscribedEmails = await _context.Customers
            .Where(c => c.Subscribe)
            .Select(c => c.Email)
            .ToListAsync();

            // 使用 Google Mail Server 發信
            string account = _configuration["EmailSettings:Account"];
            string password = _configuration["EmailSettings:Password"];
            string ReceiveMail = string.Join(",", subscribedEmails);

            string SmtpServer = "smtp.gmail.com";
            int SmtpPort = 587;
            MailMessage mms = new MailMessage();
            mms.From = new MailAddress(account);
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
                        <h1>最新消息來了！</h1>
                        <p>親愛的用戶，G++g開新的分店啦~ 邀請您與我們一起盛大迎接5月20日的公益旗艦店開幕:在開幕慶當日，不限消費金額即送精美小禮物。</p>
                        <p>立即訪問我們的網站以了解更多詳情： <a href=""https://example.com"">https://example.com</a></p>
                        <p>誠摯邀請您一同參與我們的開幕</p>
                    </body>
                </html>"; ;
            mms.IsBodyHtml = true;
            mms.SubjectEncoding = Encoding.UTF8;
            
            using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(account, password);//寄信帳密 
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
