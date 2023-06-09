using MVCAssessment2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Configuration Root
IConfigurationRoot configuration; //normal variable
configuration = new ConfigurationBuilder().AddJsonFile("./config.json").Build();

/*
// Configuration Root
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("config.json")
    .Build();
*/

// Add the connection string
builder.Services.AddDbContext<CSIROContext>(options =>
{
    var connectionString = configuration.GetConnectionString("DBConnection");
    options.UseSqlServer(connectionString);
});

// Create a user
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedEmail = true;    //add email confirmation
    })
    .AddEntityFrameworkStores<CSIROContext>()
    .AddDefaultTokenProviders();

//APIs for registering session management
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=Home}/{action=Index}/{id?}");

//pattern: "{controller=Applicant}/{action=DisplayOne}/{id?}");
pattern: "{controller=Applicant}/{action=Add}/{id?}");
//pattern: "{controller=Applicant}/{action=Edit}/{id?}");
//pattern: "{controller=Applicant}/{action=Delete}/{id?}");

//pattern: "{controller=Account}/{action=Register}/{id?}");
//pattern: "{controller=Account}/{action=Login}/{id?}");

//pattern: "{controller=Admin}/{action=CreateRole}/{id?}");
//pattern: "{controller=Admin}/{action=ManageRole}/{id?}");

//pattern: "{controller=Application}/{action=Successful}/{id?}");


// Access the "MinimumGPA" value
//var minimumGPA = configuration.GetValue<double>("MinimumGPA");

app.Run();
