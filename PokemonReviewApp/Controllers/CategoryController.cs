using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dtos;
using PokemonReviewApp.Entity;
using PokemonReviewApp.Repository.Contracts;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepositorycs _categoryRepositorycs;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepositorycs categoryRepositorycs, IMapper mapper)
        {
            _categoryRepositorycs = categoryRepositorycs;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories() 
        {
            var categories = _mapper.Map<List<CategoryDto>>(
                await _categoryRepositorycs.GetCategories());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(categories);
        }

        [HttpGet("{categoryId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Category>> GetCategory(int categoryId)
        {
            var category = _mapper.Map<CategoryDto>(
                await _categoryRepositorycs.GetCategory(categoryId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(category);
        }

        [HttpGet("pokemon/{categoryId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<Pokemon>>> GetPokemonByCategoryId(int categoryId)
        {
            var pokemonByCategory = _mapper.Map<List<PokemonDto>>(
                await _categoryRepositorycs.GetPokemonByCategory(categoryId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemonByCategory);
        }
    }
}
