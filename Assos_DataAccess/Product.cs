using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
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
    [JsonObject(IsReference = true)]
    public class Product
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        [DataMember]

        public string Name { get; set; }
        [DataMember]

        public string Description { get; set; }
        
        [DataMember]
        public bool ShopFavorites { get; set; }
        [DataMember]
        public bool CustomerFavorites { get; set; }
        [DataMember]
        public string Color { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
        [DataMember]
        public int CategoryId { get; set; }
        [DataMember]
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [DataMember]
        public ICollection<ProductPrice> ProductPrices { get; set; }
    }
}
