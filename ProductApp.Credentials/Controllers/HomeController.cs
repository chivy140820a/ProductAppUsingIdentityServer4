using Microsoft.AspNetCore.Mvc;
using ProductApp.Credentials.ConnectAPI2;
using ProductApp.Credentials.Models;
using System.Diagnostics;

namespace ProductApp.Credentials.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductConnectAPI2 _productConnectAPI2;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IProductConnectAPI2 productConnectAPI2)
        {
            _productConnectAPI2 = productConnectAPI2;
            _logger = logger;
        }

        public IActionResult Index()
        {
           
            return View();
        }
        public async Task<IActionResult> Connect()
        {
            var connect = await _productConnectAPI2.GetAll();
            return View(connect);
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