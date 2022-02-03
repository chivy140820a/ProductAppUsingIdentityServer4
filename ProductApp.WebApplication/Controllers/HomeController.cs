using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApp.WebApplication.ConnectAPI;
using ProductApp.WebApplication.Models;
using System.Diagnostics;

namespace ProductApp.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductConnectAPI _productConnectAPI;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IProductConnectAPI productConnectAPI)
        {
            _productConnectAPI = productConnectAPI;
            _logger = logger;
        }
        [Authorize]
        public IActionResult Index()
        {
            var user = User.Identity.Name;
            return View();
        }
        [Authorize(Policy = "Demo")]
        public async Task<IActionResult> Connect()
        {
            var user = await _productConnectAPI.GetAll();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}