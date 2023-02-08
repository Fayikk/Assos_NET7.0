using AssosWeb_Client.ViewModels;

namespace AssosWeb_Client.Service.IService
{
    public interface ICartService
    {
        Task DecrementCart(ShoppingCart cart);
        Task IncrementCart(ShoppingCart cart);
    }
}
