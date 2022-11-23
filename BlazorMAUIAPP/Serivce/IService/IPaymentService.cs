using DD_Models;
using DDWeb_Client.ViewModels;

namespace DDWeb_Client.Serivce.IService
{
    public interface IPaymentService
    {
        public Task<SuccessModelDTO> Checkout(StripePaymentDTO model);

    }
}
