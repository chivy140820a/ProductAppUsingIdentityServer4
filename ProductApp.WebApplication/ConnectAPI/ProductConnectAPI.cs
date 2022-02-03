using Newtonsoft.Json;
using ProductApp.ViewModel.ProductVM;

namespace ProductApp.WebApplication.ConnectAPI
{
    public class ProductConnectAPI : IProductConnectAPI
    {
        private readonly HttpClient _httpClient;
        public ProductConnectAPI(HttpClient  httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ProductViewModel>> GetAll()
        {
            var content = await _httpClient.GetAsync("/api/Product/GetAll");
            var read = await content.Content.ReadAsStringAsync();
            var mapvm = JsonConvert.DeserializeObject<List<ProductViewModel>>(read);
            return mapvm;
        }
    }
    
}
