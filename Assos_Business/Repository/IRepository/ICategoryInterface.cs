using AssosModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assos_Business.Repository.IRepository
{
    public interface ICategoryInterface
    {
        public Task<CategoryDTO> Create(CategoryDTO objDto);
        public Task<CategoryDTO> Update(CategoryDTO objDto);
        public Task<int> Delete(int id);  
        public Task<CategoryDTO> Get(int id);
        public Task<IEnumerable<CategoryDTO>> GetAll();
    
    }
}
