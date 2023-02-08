using Assos_Business.Repository.IRepository;
using AssosModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssosWeb_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository; 
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productRepository.GetAll();
            return Ok(result);   
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id==null || id==0)
            {
                return BadRequest(new ErrorModelDTO()
                {
                    ErrorMessage ="Invalid Id",
                    StatusCode=StatusCodes.Status400BadRequest
                }) ;
            }

            var product = await _productRepository.Get(id.Value);

            if (product == null)
            {
                return BadRequest(new ErrorModelDTO()
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            return Ok(product);
        }
    
    
    }
}
