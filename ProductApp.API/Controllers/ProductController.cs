using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApp.API.SerViceAPI;

namespace ProductApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductSerVice _productSerVice;
        public ProductController(IProductSerVice productSerVice)
        {
            _productSerVice = productSerVice;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _productSerVice.GetAll();
            return Ok(list);
        }    
    }
}
