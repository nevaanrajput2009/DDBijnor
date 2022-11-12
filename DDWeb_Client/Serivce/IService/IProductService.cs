using DD_Models;

namespace DDWeb_Client.Serivce.IService
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDTO>> GetAll();
        public Task<ProductDTO> Get(int productId);

        public Task<UserAddressDTO> GetDefaultAddress(string userId);
    }
}
