using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Assos_DataAccess
{
    [DataContract(IsReference = true)]
    public class ProductPrice
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [DataMember]
        public Product Product { get; set; }
        [DataMember]
        public string Size { get; set; }
        
        [DataMember]
        public double Price { get; set; }
    }
}
