 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

using System.Data.Entity;
using website.Models;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<bookstoreDBconext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

 
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
  
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
