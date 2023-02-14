using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.Serialization;

namespace Assos_DataAccess
{
    [DataContract(IsReference = true)]
    public class OrderDetail
    {
        [DataMember]

        public int Id { get; set; }
        [Required]
        [DataMember]

        public int OrderHeaderId { get; set; }
        [DataMember]

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [NotMapped]
        [DataMember]

        public virtual Product Product { get; set; }
        [DataMember]

        [Required]
        public int Count { get; set; }
        [DataMember]

        [Required]
        public double Price { get; set; }
        [DataMember]

        [Required]
        public string Size { get; set; }
        [DataMember]

        [Required]
        public string ProductName { get; set; }
    }
}
