using Assos_Business.Repository;
using Assos_Business.Repository.IRepository;
using Assos_DataAccess.Data;
using AssosWeb_Server.Data;
using AssosWeb_Server.Service;
using AssosWeb_Server.Service.IService;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt/QHRqVVhkVFpHaVZdX2NLfUN/T2FQdVt1ZCQ7a15RRnVfQF1kSXdQdEBgXXpbdA==;Mgo+DSMBPh8sVXJ0S0J+XE9AflRBQmpWfFN0RnNQdVx5flBDcC0sT3RfQF5jS35UdkxgW3pddH1SQg==;ORg4AjUWIQA/Gnt2VVhkQlFacldJXnxAYVF2R2BJdlRyfV9DZUwgOX1dQl9gSXxTcUVqWn1bc3FUQmQ=;MTA4MTEzM0AzMjMwMmUzNDJlMzBNSUdUd0swcjlUM25VVXVwZTZ2dmczYlJXYWNTWWthMWZKcDdrbHF1MWdJPQ==;MTA4MTEzNEAzMjMwMmUzNDJlMzBSanRzZVJQcmpPUy96ZGZ1TGFVaFdoVFN4b3NYOFhMRHZ0QWpkMjRZd3YwPQ==;NRAiBiAaIQQuGjN/V0Z+WE9EaFtKVmBWd0x0RWFab1d6dlxMZVlBNQtUQF1hSn5RdkJjUHxcdXdTR2RY;MTA4MTEzNkAzMjMwMmUzNDJlMzBTZEQ5T1pnQ2hhKzl4a1BnYWYvcnRQK2daeWNBdEwrYm1yNmRPRE93N1RVPQ==;MTA4MTEzN0AzMjMwMmUzNDJlMzBhQktYVzllTGpZMkZidWliZU5oNlN3SXN4SW54Yjh4VkFNaDFpWUc0cGxrPQ==;Mgo+DSMBMAY9C3t2VVhkQlFacldJXnxAYVF2R2BJdlRyfV9DZUwgOX1dQl9gSXxTcUVqWn1bc3xVRWM=;MTA4MTEzOUAzMjMwMmUzNDJlMzBUeG5vdkFUeTlpQWtwSTZhYitpd1JySStsZHZyL2crVDB0ZzdnZ3g0QXAwPQ==;MTA4MTE0MEAzMjMwMmUzNDJlMzBJM0hTUVRuMjVtY3ZnYzk5UWRXeXVQSG9MTmx0amlFbjZLNVBIYUpUVkNzPQ==;MTA4MTE0MUAzMjMwMmUzNDJlMzBTZEQ5T1pnQ2hhKzl4a1BnYWYvcnRQK2daeWNBdEwrYm1yNmRPRE93N1RVPQ==");

var builder = WebApplication.CreateBuilder(args);
var serviceCollection = new ServiceCollection();
//For syncfusion

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddServerSideBlazor();



//For AddScoped Process
builder.Services.AddScoped<ICategoryInterface, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IProductPriceRepository, ProductPriceRepository>().Reverse();
builder.Services.AddScoped<IFileUpload, FileUpload>();

//serviceCollection.AddScoped<ICategoryInterface, CategoryRepository>();
//serviceCollection.AddScoped<IProductPriceRepository, ProductPriceRepository>();
//serviceCollection.AddScoped<IProductRepository, ProductRepository>();
//serviceCollection.AddScoped<IFileUpload, FileUpload>();

builder.Services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser,IdentityRole>()
    .AddDefaultTokenProviders().AddDefaultUI().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());//For enable AutoMapper

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
SeedDatabase();
app.UseAuthentication();
app.UseAuthorization();


app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}