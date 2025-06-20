using ImperiumAuctions.Data;
using Microsoft.EntityFrameworkCore;
using ImperiumAuctions.Repository.IRepository;
using ImperiumAuctions.Repository;
using Microsoft.AspNetCore.Identity;
using ImperiumAuctions.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using ImperiumAuctions.DbInitializer;
using ImperiumAuctions.Communication;

var builder = WebApplication.CreateBuilder(args);


// Configure lowercase URLs
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    //options.LowercaseQueryStrings = true; // optional
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddIdentity<IdentityUser,IdentityRole>(options=>options.SignIn.RequireConfirmedAccount=true).AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";

});

builder.Services.AddAuthentication().AddFacebook(option =>
{
    option.AppId = "1046321050521419";
    option.AppSecret = "e7504e8414b94b51d2b59683640e3d12";
});

builder.Services.AddScoped<IMainRepository, MainRepository>();
builder.Services.AddScoped<IEmailSender,EmailSender>();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
// Add SignalR service
builder.Services.AddSignalR();

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
StripeConfiguration.ApiKey=builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
SeedDatabase();
app.MapRazorPages();

// Map SignalR hubs
app.MapHub<UpdateBidSystem>("/updateBidSystem");

app.MapControllerRoute(
    name: "default",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var dbInitializer = services.GetRequiredService<IDbInitializer>();
            dbInitializer.Initialize();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}