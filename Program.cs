using Bestshop.Models;
using Bestshop.MyHelpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
// Register IEmailSender service
builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(36000);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Add services to the container.
builder.Services.AddDbContext<ThayHuyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ThayHuyDatabase")));

var app = builder.Build();

//Cấu hình Email Sender
//static void ConfigureServices(IServiceCollection services)
//{
//    services.AddRazorPages();

//    // Add EmailSender service
//    services.AddScoped<IEmailSender, EmailSender>();
//    // Register IEmailSender service
//    services.AddTransient<IEmailSender, EmailSender>();
//}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();
