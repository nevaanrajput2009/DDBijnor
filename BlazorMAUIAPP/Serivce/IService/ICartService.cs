using DDWeb_Client.ViewModels;

namespace DDWeb_Client.Serivce.IService
{
    public interface ICartService
    {
        public event Action OnChange;
        Task DecrementCart(ShoppingCart shoppingCart);
        Task IncrementCart(ShoppingCart shoppingCart);

    }
}
