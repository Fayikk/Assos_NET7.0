using Assos_DataAccess;
using AssosModels;
using AssosWeb_Client.Service.IService;
using Newtonsoft.Json;
namespace AssosWeb_Client.Service
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private string BaseServerUrl;
        public ProductService(HttpClient httpClient,IConfiguration config)
        {
            _config = config;
            _httpClient = httpClient;
            BaseServerUrl = _config.GetSection("BaseServerUrl").Value;
        }

        public async Task<ProductDTO> Get(int productId)
        {
            var response = await _httpClient.GetAsync($"/api/product/{productId}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var product =  JsonConvert.DeserializeObject<ProductDTO>(content);
                product.ImageUrl = BaseServerUrl + product.ImageUrl;
                return product;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(content);
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            var response = await _httpClient.GetAsync("/api/product");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(content);
                foreach (var prod in products)
                {
                    prod.ImageUrl=BaseServerUrl+prod.ImageUrl;
                }
                return products;
            }

            return new List<ProductDTO>();
        }
    }
}
