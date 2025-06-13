// Program.cs
using Microsoft.EntityFrameworkCore;
using mmrcis.Data;   // Ensure this matches your project's Data namespace
using mmrcis.Models; // Ensure this matches your project's Models namespace
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Add services to the container. ---

// Configure your DbContext for SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<CisDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure ASP.NET Core Identity
// Uses ApplicationUser as the user type and IdentityRole as the role type
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>() // Enable role management for Identity
    .AddEntityFrameworkStores<CisDbContext>(); // Tell Identity to use your CisDbContext to store identity data

// Add MVC services (controllers and views)
builder.Services.AddControllersWithViews();

// --- Configure Authorization Policies (Optional but Recommended for clearer access control) ---
// These policies can be used with [Authorize(Policy = "PolicyName")] attributes
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireMedicalRole", policy => policy.RequireRole("Doctor", "Nurse"));
    options.AddPolicy("RequireOperatorRole", policy => policy.RequireRole("Operator"));
    options.AddPolicy("RequireStaff", policy => policy.RequireRole("Admin", "Doctor", "Nurse", "Operator")); // Any staff role
});


var app = builder.Build();

// --- 2. Configure the HTTP request pipeline. ---

// Configure error handling for non-development environments
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
app.UseStaticFiles();      // Serve static files (CSS, JavaScript, images from wwwroot)

app.UseRouting(); // Enables endpoint routing

// Authentication and Authorization middleware. Order is crucial:
// UseAuthentication must come before UseAuthorization
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
// --- Configure Area Routes ---
// Area routes should be defined BEFORE the default route, as they are more specific.
// The order here determines precedence.
app.MapControllerRoute(
    name: "AdminArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "MedicalArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "OperatorArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Default fallback route for non-area controllers (e.g., Home, Persons, CostRates)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// --- Database Seeding ---
// This block ensures that the database is migrated and seeded when the application starts.
// It creates a service scope to resolve DbContext, UserManager, and RoleManager.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CisDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Call the static Initialize method from your DbInitializer
        await DbInitializer.Initialize(context, userManager, roleManager);
    }
    catch (Exception ex)
    {
        // If an error occurs during seeding, log it.
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run(); // Run the application
