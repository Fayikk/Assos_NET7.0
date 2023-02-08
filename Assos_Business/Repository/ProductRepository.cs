using Assos_Business.Repository.IRepository;
using Assos_DataAccess;
using Assos_DataAccess.Data;
using AssosModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Assos_Business.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ProductRepository(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;   
        }

        public async Task<ProductDTO> Create(ProductDTO objDto)
        {
            var obj = _mapper.Map<ProductDTO, Product>(objDto);//Eşleme 
             var addedObj =  _context.Products.Add(obj);
           await _context.SaveChangesAsync();
            return _mapper.Map<Product, ProductDTO>(addedObj.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var obj = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (obj != null)
            {
                _context.Remove(obj);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<ProductDTO> Get(int id)
        {
            var obj = await _context.Products.Include(u => u.Category).Include(u => u.ProductPrices).FirstOrDefaultAsync(u => u.Id == id);
            if (obj != null)
            {
                return _mapper.Map<Product, ProductDTO>(obj);
            }
            return new ProductDTO();
        }

        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(_context.Products.Include(u => u.Category).Include(u => u.ProductPrices));
            return result ;
        }

        public async Task<ProductDTO> Update(ProductDTO objDto)
        {
            var obj = await _context.Products.FirstOrDefaultAsync(x=>x.Id == objDto.Id);

            if(obj != null)
            {
                obj.Name=objDto.Name;
                obj.Description=objDto.Description;
                obj.ImageUrl = objDto.ImageUrl;
                obj.CategoryId = objDto.CategoryId;
                obj.Color = objDto.Color;
                obj.ShopFavorites = objDto.ShopFavourites;
                obj.CustomerFavorites = objDto.CustomerFavourites;
                _context.Products.Update(obj);
              await  _context.SaveChangesAsync();
                return _mapper.Map<Product, ProductDTO>(obj);

            }
            return objDto;
        }
    }
}
