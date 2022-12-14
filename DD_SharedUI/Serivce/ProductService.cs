using DD_Models;
using DD_SharedUI.Serivce.IService;
using DD_SharedUI.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DD_SharedUI.Serivce
{
    public class ProductService : IProductService
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string BaseServerUrl;
        public ProductService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            BaseServerUrl = _configuration.GetSection("BaseServerUrl").Value;
        }

        public async Task<ProductDTO> Get(int productId)
        {
            var response = await _httpClient.GetAsync($"/api/product/{productId}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var product = JsonConvert.DeserializeObject<ProductDTO>(content);
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
            var response = await _httpClient.GetAsync($"/api/product");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(content);
                foreach (var prod in products)
                {
                    prod.ImageUrl = BaseServerUrl + prod.ImageUrl;
                }
                return products;
            }

            return new List<ProductDTO>();
        }

        public async Task<UserAddressDTO> GetDefaultAddress(string userId)
        {
            var response = await _httpClient.GetAsync($"/api/account/GetUserAddress/{userId}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var userAddress = JsonConvert.DeserializeObject<UserAddressDTO>(content);
                return userAddress;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(content);
                throw new Exception(errorModel.ErrorMessage);
            }
        }

    }
}
