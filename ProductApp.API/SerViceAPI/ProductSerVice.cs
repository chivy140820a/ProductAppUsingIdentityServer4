using Microsoft.EntityFrameworkCore;
using ProductApp.API.Entity;

namespace ProductApp.API.SerViceAPI
{
    public class ProductSerVice : IProductSerVice
    {
        private readonly AppDbContext _context;
        public ProductSerVice(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetAll()
        {
            var getall = await _context.Products.ToListAsync();
            return getall;
        }
    }
}
