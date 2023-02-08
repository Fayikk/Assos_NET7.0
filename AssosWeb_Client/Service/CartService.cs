using Assos_Common;
using AssosWeb_Client.Service.IService;
using AssosWeb_Client.ViewModels;
using Blazored.LocalStorage;

namespace AssosWeb_Client.Service
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        public CartService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;   
        }

        public async Task DecrementCart(ShoppingCart cartToDelete)
        {
            var cart = await _localStorage.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);

            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].ProductId == cartToDelete.ProductId && cart[i].ProductPriceId == cartToDelete.ProductPriceId)
                {
                    if (cart[i].Count == 1 || cart[i].Count==0)
                    {
                        cart.Remove(cart[i]);
                    }
                    else
                    {
                        cart[i].Count -= cartToDelete.Count;
                    }
                }
            }
        
            await _localStorage.SetItemAsync(SD.ShoppingCart, cart);

        }

        public async Task IncrementCart(ShoppingCart cartToAdd)
        {
            var cart = await _localStorage.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);
            bool itemInCart = false;
            if (cart == null)
            {
                cart = new List<ShoppingCart>();
            }
            foreach (var item in cart)
            {
                if (item.ProductId== cartToAdd.ProductId && item.ProductPriceId==cartToAdd.ProductPriceId)
                {
                    itemInCart = true;
                    item.Count = item.Count + cartToAdd.Count;
                }
            }
            if (!itemInCart)
            {
                cart.Add(new ShoppingCart()
                {
                    ProductId = cartToAdd.ProductId,
                    ProductPriceId = cartToAdd.ProductPriceId,
                    Count = cartToAdd.Count
                });
            }
            await _localStorage.SetItemAsync(SD.ShoppingCart, cart);


        }
    }
}
