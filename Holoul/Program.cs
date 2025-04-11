using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Holoul.Areas.Identity.Data;
using Microsoft.Extensions.Configuration;
using Holoul.services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("EDbContextConnection")
    ?? throw new InvalidOperationException("Connection string 'EDbContextConnection' not found.");

builder.Services.AddDbContext<EDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<HoloulUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<EDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Services.Configure<GeminiSettings>(builder.Configuration.GetSection("Gemini"));
builder.Services.AddScoped<GeminiService>();



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var RoleManager =
        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "User" };
    foreach (var role in roles)
    {
        if (!await RoleManager.RoleExistsAsync(role))
            await RoleManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager =
        scope.ServiceProvider.GetRequiredService<UserManager<HoloulUser>>();
    string email = "AdminHoloul@Admin.com";
    string email1 = "TestHoloul@User.com";
    string password = "Test@1234";


    if (await userManager.FindByEmailAsync(email) == null)
    {

        var user = new HoloulUser();

        user.UserName = email;
        user.Email = email;
        user.EmailConfirmed = true;
        user.FirstName = "Admin";
        user.LastName = "User";
        await userManager.CreateAsync(user, password);

        await userManager.AddToRoleAsync(user, "Admin");

    }

    if (await userManager.FindByEmailAsync(email1) == null)
    {

        var user = new HoloulUser();

        user.UserName = email1;
        user.Email = email1;
        user.EmailConfirmed = true;
        user.FirstName = "User";
        user.LastName = "Test";
        await userManager.CreateAsync(user, password);

        await userManager.AddToRoleAsync(user, "User");

    }
}

app.Run();

public class GeminiSettings
{
    public string? ApiKey { get; set; }
}