using AssosModels;

namespace AssosWeb_Client.Service.IService
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderDTO>> GetAll();
        public Task<OrderDTO> Get(int orderId);
        public Task<OrderDTO> Create(StripePaymentDTO paymentDTO);
        public Task<OrderHeaderDTO> MarkPaymentSuccessfull(OrderHeaderDTO orderHeader);
    }
}
