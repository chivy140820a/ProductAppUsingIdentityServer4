using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.Authorization;
using ProductApp.WebApplication.ConnectAPI;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews(o => o.Filters.Add(new AuthorizeFilter()));


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
        options.ClientId = "bookstore_webapp";
        options.ClientSecret = "supersecret";
        options.CallbackPath = "/signin-oidc";

        options.Scope.Add("openid");
        options.Scope.Add("bookstore");

        options.Scope.Add("bookstore_apis");

        //options.Scope.Add("offline_access");

        options.SaveTokens = true;
        //options.GetClaimsFromUserInfoEndpoint = true;
        //options.ClaimActions.MapUniqueJsonKey("UserName", "UserName");
        //options.ClaimActions.MapUniqueJsonKey("PhoneNumber", "PhoneNumber");
        //options.ClaimActions.MapUniqueJsonKey("Email", "Email");
        //options.ClaimActions.MapUniqueJsonKey("Permission", "Permission");
        //options.ClaimActions.MapUniqueJsonKey("Role", "Role");


        options.ResponseType = "code";
        options.ResponseMode = "form_post";

        options.UsePkce = true;
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Demo", policy => policy.RequireClaim("Permission","CanRead"));
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<IProductConnectAPI, ProductConnectAPI>(
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
