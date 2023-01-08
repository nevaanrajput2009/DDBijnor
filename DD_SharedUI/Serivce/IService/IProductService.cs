using DD_Models;

namespace DD_SharedUI.Serivce.IService
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDTO>> GetAll();
        public Task<ProductDTO> Get(int productId);

        public Task<UserAddressDTO> GetDefaultAddress(string userId);
    }
}
