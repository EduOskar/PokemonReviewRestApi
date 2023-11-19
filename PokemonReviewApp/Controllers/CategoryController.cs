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

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> CreateCategory([FromBody] CategoryDto categoryCreate)
        {
            if (categoryCreate == null)
            {
                return BadRequest(ModelState);
            }

            var category = await _categoryRepositorycs.GetCategories();
            var categoryname = category
                .Where(c => c.Name!.Trim().ToUpper() == categoryCreate.Name!.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (categoryname != null)
            {
                ModelState.AddModelError("", "Category already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryMap = _mapper.Map<Category>(categoryCreate);

            if (! await _categoryRepositorycs.CreateCategory(categoryMap)) 
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully created");
                
        } 
    }
}
