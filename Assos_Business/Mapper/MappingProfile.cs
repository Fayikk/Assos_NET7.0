using Assos_DataAccess;
using AssosModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assos_Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Product,ProductDTO>().ReverseMap();
            CreateMap<ProductPrice, ProductPriceDTO>().ReverseMap();
        }
    }
}
