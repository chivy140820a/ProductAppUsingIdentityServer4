using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ProductApp.IdentityProvider.Data;
using ProductApp.IdentityProvider.SerVice;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
               .AddDefaultTokenProviders()
               .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddMvc();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddIdentityServer()
    .AddInMemoryIdentityResources(builder.Configuration.GetSection("IdentityServer:IdentityResources"))
     .AddInMemoryApiScopes(builder.Configuration.GetSection("IdentityServer:ApiScopes"))
    .AddInMemoryApiResources(builder.Configuration.GetSection("IdentityServer:ApiResources"))
    .AddInMemoryClients(builder.Configuration.GetSection("IdentityServer:Clients"))
    .AddDeveloperSigningCredential()
    .AddAspNetIdentity<IdentityUser>();
builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()));
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAll");
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
