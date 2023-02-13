using System.ComponentModel.DataAnnotations;

namespace AssosModels
{
    public class ProductPriceDTO
    {
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string Size { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 1")]
        public double Price { get; set; }
    }
}
//Self referencing loop detected with type 'Assos_DataAccess.ProductPrice'. Path 'Order.OrderDetails[0].Product.ProductPrices[0].Product.ProductPrice