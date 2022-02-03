using ProductApp.API.Entity;

namespace ProductApp.API.SerViceAPI
{
    public interface IProductSerVice
    {
        Task<List<Product>> GetAll();
    }
}
