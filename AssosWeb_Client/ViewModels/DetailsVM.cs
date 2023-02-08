using Assos_DataAccess;
using AssosModels;
using System.ComponentModel.DataAnnotations;

namespace AssosWeb_Client.ViewModels
{

    public class DetailsVM
    {
        public DetailsVM()
        {
            ProductPrice = new();
            Count = 1;
        }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than 0")]
        public int Count { get; set; }
        [Required]
        public int SelectedProductPriceId { get; set; }
        public ProductPrice ProductPrice { get; set; }
    }



}
