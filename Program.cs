
using Microsoft.EntityFrameworkCore;
using mmrcis.Data;   
using mmrcis.Models; 
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);




var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<CisDbContext>(options =>
    options.UseSqlServer(connectionString));



builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>() 
    .AddEntityFrameworkStores<CisDbContext>(); 


builder.Services.AddControllersWithViews();



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireMedicalRole", policy => policy.RequireRole("Doctor", "Nurse"));
    options.AddPolicy("RequireOperatorRole", policy => policy.RequireRole("Operator"));
    options.AddPolicy("RequireStaff", policy => policy.RequireRole("Admin", "Doctor", "Nurse", "Operator")); 
});


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


app.MapRazorPages();



app.MapControllerRoute(
    name: "AdminArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "MedicalArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "OperatorArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");




using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CisDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        
        await DbInitializer.Initialize(context, userManager, roleManager);
    }
    catch (Exception ex)
    {
        
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run(); 
