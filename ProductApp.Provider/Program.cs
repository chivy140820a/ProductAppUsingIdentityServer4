using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
               .AddDefaultTokenProviders()
               .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentityServer()
               .AddInMemoryIdentityResources(builder.Configuration.GetSection("IdentityServer:IdentityResources"))
                .AddInMemoryApiScopes(builder.Configuration.GetSection("IdentityServer:ApiScopes"))
               .AddInMemoryApiResources(builder.Configuration.GetSection("IdentityServer:ApiResources"))
               .AddInMemoryClients(builder.Configuration.GetSection("IdentityServer:Clients"))
               .AddDeveloperSigningCredential()
               .AddAspNetIdentity<IdentityUser>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
