using Assos_DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssosModels
{
    public class StripePaymentDTO
    {

        public StripePaymentDTO()
        {
            SuccessUrl = "orderconfirmation";
            CancelUrl= "summary";
        }

        public OrderDTO Order { get; set; }
        public string SuccessUrl { get; set; }  
        public string CancelUrl { get; set; }
    }
}
