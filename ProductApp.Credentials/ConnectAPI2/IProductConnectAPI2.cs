using ProductApp.ViewModel.ProductVM;

namespace ProductApp.Credentials.ConnectAPI2
{
    public interface IProductConnectAPI2
    {
        Task<List<ProductViewModel>> GetAll();
    }
}
