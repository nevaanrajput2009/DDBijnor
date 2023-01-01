using Newtonsoft.Json;
using System.Text;
using DD_Models;
using DDWeb_Client.Serivce.IService;
using DDWeb_Client.Helper;

namespace DDWeb_Client.Serivce
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration;
        private string BaseServerUrl;
        public OrderService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration=configuration;
            BaseServerUrl = _configuration.GetSection("BaseServerUrl").Value;
        }

        public async Task<OrderDTO> Create(StripePaymentDTO paymentDTO)
        {
            paymentDTO.Order.OrderHeader.State = paymentDTO.Order.OrderHeader.City;
            paymentDTO.Order.OrderHeader.PostalCode = "246721"; //paymentDTO.Order.OrderHeader.PostalCode,
            var content = JsonConvert.SerializeObject(paymentDTO);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{IISHelper.AppentUrl}/api/order/create", bodyContent);
            string responseResult = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                // updaate user address
                var userAddressDTO = new UserAddressDTO()
                {
                    Name = paymentDTO.Order.OrderHeader.Name,
                    PhoneNumber = paymentDTO.Order.OrderHeader.PhoneNumber,
                    StreetAddress = paymentDTO.Order.OrderHeader.StreetAddress,
                    City = paymentDTO.Order.OrderHeader.City,
                    State = paymentDTO.Order.OrderHeader.State,
                    PostalCode = paymentDTO.Order.OrderHeader.PostalCode,
                    UserId = paymentDTO.Order.OrderHeader.UserId
                };
                var content1 = JsonConvert.SerializeObject(userAddressDTO);
                var bodyContent1 = new StringContent(content1, Encoding.UTF8, "application/json");
                var response1 = await _httpClient.PostAsync($"{IISHelper.AppentUrl}/api/account/UpdateUserAddress", bodyContent1);
                var responseResultUser = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<OrderDTO>(responseResult);
                return result;
            }
            return new OrderDTO();

        }

        public async Task<OrderDTO> Get(int orderHeaderId)
        {
            var response = await _httpClient.GetAsync($"{IISHelper.AppentUrl}/api/order/Get/{orderHeaderId}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var order = JsonConvert.DeserializeObject<OrderDTO>(content);
                return order;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(content);
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<IEnumerable<OrderDTO>> GetAll(string? userId=null)
        {
            var response = await _httpClient.GetAsync($"{IISHelper.AppentUrl}/api/Order/GetAll/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(content);

                return orders;
            }

            return new List<OrderDTO>();
        }

        public async Task<bool> UpdateOrderStatus(int orderId, string status)
        {
            var content = JsonConvert.SerializeObject(new OrderStatusDTO { OrderId = orderId, Status = status });
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{IISHelper.AppentUrl}/api/order/updatestatus", bodyContent);
            string responseResult = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                var isOrderStatusUpdated = JsonConvert.DeserializeObject<bool>(responseResult);
                return isOrderStatusUpdated;
            }
            var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(responseResult);
            throw new Exception(errorModel?.ErrorMessage);
        }

        public async Task<bool> NotifyOrderChecked(int orderId)
        {
            var content = JsonConvert.SerializeObject(new OrderStatusDTO { OrderId = orderId, Status = "Confirmed" });
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{IISHelper.AppentUrl}/api/order/notifyOrderChecked", bodyContent);
            string responseResult = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                var isOrderStatusUpdated = true;
                return isOrderStatusUpdated;
            }
            var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(responseResult);
            throw new Exception(errorModel?.ErrorMessage);
        }        

        public async Task<OrderHeaderDTO> MarkPaymentSuccessful(OrderHeaderDTO orderHeader)
        {
            var content = JsonConvert.SerializeObject(orderHeader);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{IISHelper.AppentUrl}/api/order/paymentsuccessful", bodyContent);
            string responseResult = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<OrderHeaderDTO>(responseResult);
                return result;
            }
            var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(responseResult);
            throw new Exception(errorModel.ErrorMessage);
        }
    }
}
