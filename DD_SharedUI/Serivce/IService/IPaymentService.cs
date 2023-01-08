using DD_Models;
using DD_SharedUI.ViewModels;

namespace DD_SharedUI.Serivce.IService
{
    public interface IPaymentService
    {
        public Task<SuccessModelDTO> Checkout(StripePaymentDTO model);

    }
}
