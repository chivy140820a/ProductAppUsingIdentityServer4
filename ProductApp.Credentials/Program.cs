using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using ProductApp.Credentials.ConnectAPI2;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddOpenIdConnect(options =>
    {
        options.Authority = "https://localhost:44379";
        options.ClientId = "bookseller";
        options.ClientSecret = "easypassword";
        options.Scope.Add("bookstore_apis");
        //options.Scope.Add("offline_access");
        options.SaveTokens = true;


        options.ResponseType = "client_credentials";
        options.ResponseMode = "form_post";

    });
builder.Services.AddHttpClient<IProductConnectAPI2, ProductConnectAPI2>(
   async (c, client) =>
   {
       var accessor = c.GetRequiredService<IHttpContextAccessor>();
       var accessToken = await accessor.HttpContext.GetTokenAsync("access_token");
       client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
       client.BaseAddress = new Uri("https://localhost:44393");
   });
builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
 .AllowAnyMethod()
 .AllowAnyHeader()));

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
app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
