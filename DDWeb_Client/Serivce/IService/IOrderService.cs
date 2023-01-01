using DD_Models;

namespace DDWeb_Client.Serivce.IService
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderDTO>> GetAll(string? userId);
        public Task<OrderDTO> Get(int orderId);

        public Task<OrderDTO> Create(StripePaymentDTO paymentDTO);
        public Task<bool> UpdateOrderStatus(int orderId, string status);
        public Task<bool> NotifyOrderChecked(int orderId);
        public Task<OrderHeaderDTO> MarkPaymentSuccessful(OrderHeaderDTO orderHeader);
    }
}
