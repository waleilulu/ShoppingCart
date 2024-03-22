using Microsoft.EntityFrameworkCore;
using Project0220.myModels;
using Project0220.Models;
using Microsoft.AspNetCore.Authentication.Cookies;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);//加密

//Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "membercookie";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(1); // 設定 Cookie 的過期時間
        options.LoginPath = "/Customers/Login"; // 登入頁面的路徑
        options.LogoutPath = "/Customers/Logout"; // 登出頁面的路徑
    })
	.AddCookie("userRole", options =>
	{
		options.Cookie.Name = "userRole";
		options.Cookie.HttpOnly = true;
		options.ExpireTimeSpan = TimeSpan.FromHours(1);
	//	options.LoginPath = "/Customers/Admin"; // 登入頁面的路徑
		options.LogoutPath = "/Customers/Logout"; // 登出頁面的路徑
	})
.AddCookie("isAdmin", options =>
 {
     options.Cookie.Name = "isAdmin";
     options.Cookie.HttpOnly = true;
     options.ExpireTimeSpan = TimeSpan.FromHours(1);
     //	options.LoginPath = "/Customers/Admin"; // 登入頁面的路徑
     options.LogoutPath = "/Customers/Logout"; // 登出頁面的路徑
 });
;



builder.Services.AddDbContext<ManualECommerceDBContext>(options => 
{
	var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
	options.UseSqlServer(connectionString);
});

builder.Services.AddDbContext<ScaffoldEcommerceDbContext>
	(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ECommerceDbContext")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
        
app.UseAuthorization();

app.UseAuthentication();

app.UseSession();




app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
