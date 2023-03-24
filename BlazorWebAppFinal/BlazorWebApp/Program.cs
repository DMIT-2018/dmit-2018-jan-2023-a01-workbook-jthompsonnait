using BlazorWebApp.Areas.Identity;
using BlazorWebApp.Data;
using MatBlazor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlaylistManagementSystem;
using Syncfusion.Blazor;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//  :given
//  Supplied database connection due to the fact that we create this
//      web app to use Individual Account
//  Code retrieves the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//  :added
//  Code retrieves the ChinookDB connection string
//  Code for your project, it will be GetConnectionString("EBikeDB");
var connectionStringChinook = builder.Configuration.GetConnectionString("ChinookDB");

//  given
//  Register the supplied connection string with the IServiceCollection (.Services)
//  Registers the connection string for Individual Accounts
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<ApplicationDbContext>(a =>
    {
        a.UseSqlServer(connectionStringChinook);
        a.EnableSensitiveDataLogging();
    }
);

//  added
//  Code the logic to add our class library services to IServiceCollection
//  One could to the registration code here in Program.cs
//  HOWEVER, every time a service class is added, you would be changing this file
//  The implementation of the DBContent and AddTransient(..) code in this example
//      will be done in an extension method to  IServiceCollection
//  The extension method will be coded inside the ChinookSystem class library
//  The extension method will have a parameter:  options.UseSqlServer()
//  We will name the method by the service type that we are running  ie: PurchaseOrderDependencies, SalesDependencies
builder.Services.PlaylistManagementDependencies(options => options.UseSqlServer(connectionStringChinook));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddMatBlazor();
builder.Services.AddSyncfusionBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
