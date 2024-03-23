using Microsoft.EntityFrameworkCore;
using Project0220.myModels;
using Project0220.Models;
using Microsoft.AspNetCore.Authentication.Cookies;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);//�[�K

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
        options.ExpireTimeSpan = TimeSpan.FromHours(1); // �]�w Cookie ���L���ɶ�
        options.LoginPath = "/Customers/Login"; // �n�J���������|
        options.LogoutPath = "/Customers/Logout"; // �n�X���������|
    })
	.AddCookie("userRole", options =>
	{
		options.Cookie.Name = "userRole";
		options.Cookie.HttpOnly = true;
		options.ExpireTimeSpan = TimeSpan.FromHours(1);
	//	options.LoginPath = "/Customers/Admin"; // �n�J���������|
		options.LogoutPath = "/Customers/Logout"; // �n�X���������|
	})
.AddCookie("isAdmin", options =>
 {
     options.Cookie.Name = "isAdmin";
     options.Cookie.HttpOnly = true;
     options.ExpireTimeSpan = TimeSpan.FromHours(1);
     //	options.LoginPath = "/Customers/Admin"; // �n�J���������|
     options.LogoutPath = "/Customers/Logout"; // �n�X���������|
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
