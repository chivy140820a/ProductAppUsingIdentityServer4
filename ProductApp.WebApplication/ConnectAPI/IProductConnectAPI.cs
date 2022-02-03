using ProductApp.ViewModel.ProductVM;

namespace ProductApp.WebApplication.ConnectAPI
{
    public interface IProductConnectAPI
    {
        Task<List<ProductViewModel>> GetAll();
    }
}
