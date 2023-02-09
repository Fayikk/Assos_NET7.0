using Assos_DataAccess;
using AssosModels;

namespace AssosWeb_Client.ViewModels
{
    public class ShoppingCart
    {
        public int ProductId { get; set; }  
        public ProductDTO Product { get; set; }
        public int ProductPriceId { get; set; }
        public ProductPrice ProductPrice { get; set; }
        public int Count { get; set; }
    }
}
