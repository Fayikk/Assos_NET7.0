using AssosModels;

namespace AssosWeb_Client.Service.IService
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderDTO>> GetAll();
        public Task<OrderDTO> Get(int orderId);
    }
}
