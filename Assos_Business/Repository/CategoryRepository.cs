using Assos_Business.Repository.IRepository;
using Assos_DataAccess;
using Assos_DataAccess.Data;
using AssosModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assos_Business.Repository
{
    public class CategoryRepository : ICategoryInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CategoryRepository(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;   
        }

        public async Task<CategoryDTO> Create(CategoryDTO objDto)
        {
            var obj = _mapper.Map<CategoryDTO, Category>(objDto);//Eşleme 
            obj.CreatedDate = DateTime.Now;
             var addedObj =  _context.Categories.Add(obj);
           await _context.SaveChangesAsync();
            return _mapper.Map<Category,CategoryDTO >(addedObj.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var obj = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (obj != null)
            {
                _context.Remove(obj);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<CategoryDTO> Get(int id)
        {
            var obj = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (obj != null) 
            {
                return _mapper.Map<Category, CategoryDTO>(obj);
            }
            return new CategoryDTO();
        }

        public async  Task<IEnumerable<CategoryDTO>> GetAll()
        {
            return  _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(_context.Categories);
        }

        public async Task<CategoryDTO> Update(CategoryDTO objDto)
        {
            var obj = await _context.Categories.FirstOrDefaultAsync(x=>x.Id == objDto.Id);

            if(obj != null)
            {
                obj.Name=objDto.Name;
                _context.Categories.Update(obj);
              await  _context.SaveChangesAsync();
                return _mapper.Map<Category, CategoryDTO>(obj);

            }
            return objDto;
        }
    }
}
