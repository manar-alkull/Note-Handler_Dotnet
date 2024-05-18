using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoteManager.Models;
using NoteManager.tools;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddAuthentication(options => {
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Home/Error";
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidAudience = " you site link blah blah",
                    ValidIssuer = "You Site link Blah  blah",
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("somekey123@"))
                    ,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });




// Add services to the container.
builder.Services.AddRazorPages();

//--------------------------------------------------------------------------------------------------------
//https://dotnettutorials.net/lesson/asp-net-core-identity-setup/
var connectionString = builder.Configuration.GetConnectionString("SQLServerIdentityConnection") ?? throw new InvalidOperationException("Connection string 'SQLServerIdentityConnection' not found.");

builder.Services.AddSingleton<SoftDeleteInterceptor>();

builder.Services.AddDbContext<ApplicationDbContext>((sp,options) =>
    options.UseSqlServer(connectionString)
    .AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>())
    );

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddApiEndpoints()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultUI()
                    ;

//--------------------------------------------------------------------------------------------------------


// Add services to the container.
builder.Services.AddControllersWithViews();


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

//--------------------------------------------------------------------------------------------------------

//Configuring Authentication Middleware to the Request Pipeline
app.UseAuthentication();
//--------------------------------------------------------------------------------------------------------

app.MapGroup("/account").MapIdentityApi<IdentityUser>();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapIdentityApi<IdentityUser>();
//app.MapRazorPages();

app.MapControllers();

app.Run();
