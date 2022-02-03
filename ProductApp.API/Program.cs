using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using ProductApp.API;
using ProductApp.API.SerViceAPI;
using ProductApp.IdentityProvider.Data;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(o => o.Filters.Add(new AuthorizeFilter()));

builder.Services.AddDbContext<AppDbContext>(options =>
          options.UseSqlServer(
              builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
           options.UseSqlServer(
               builder.Configuration.GetConnectionString("DefaultConnectionString")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddMvc();
builder.Services.AddAuthentication(
JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = "https://localhost:44379";
    options.Audience = "bookstore_apis";

});
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IProductSerVice,ProductSerVice>();
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
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Test1 Api v1");
});


app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
