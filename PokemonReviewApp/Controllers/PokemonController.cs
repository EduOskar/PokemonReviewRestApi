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
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICategoryRepositorycs _categoryRepositorycs;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository,
            IOwnerRepository ownerRepository,
            ICategoryRepositorycs categoryRepositorycs,
            IReviewRepository reviewRepository,
            IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _ownerRepository = ownerRepository;
            _categoryRepositorycs = categoryRepositorycs;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<Pokemon>>> GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(
                await _pokemonRepository.GetPokemons());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemons);
        }

        [HttpGet("{pokeId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Pokemon>> GetPokemon(int pokeId)
        {
            if (await _pokemonRepository.PokemonExist(pokeId) == false)
            {
                return NotFound();
            }

            var pokemon = _mapper.Map<PokemonDto>(
                await _pokemonRepository.GetPokemon(pokeId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemon);
        }

        [HttpGet("{pokeId:int}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<Pokemon>>> GetPokemonRating(int pokeId)
        {
            if (await _pokemonRepository.PokemonExist(pokeId) == false)
            {
                return NotFound();
            }

            var rating = await _pokemonRepository.GetPokemonRating(pokeId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(rating);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> CreateOwner([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDto pokemonCreate)
        {
            if (pokemonCreate == null)
            {
                return BadRequest(ModelState);
            }

            var pokemons = await _pokemonRepository.GetPokemons();
            var pokemonNames = pokemons
                .Where(p => p.Name == pokemonCreate?.Name?.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (pokemonNames != null)
            {
                ModelState.AddModelError("", "Pokemon Already Exist");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);

            if (!await _pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wron while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Pokemon was created");

        }

        [HttpPut("{pokemonId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateOwner(int pokemonId,
            [FromQuery] int ownerId, [FromQuery] int categoryId,
            [FromBody] PokemonDto updatePokemon)
        {
            if (updatePokemon == null)
            {
                return BadRequest(ModelState);
            }

            if (pokemonId != updatePokemon.Id)
            {
                return BadRequest(ModelState);
            }

            if (!await _pokemonRepository.PokemonExist(pokemonId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var pokemonMap = _mapper.Map<Pokemon>(updatePokemon);

            if (!await _pokemonRepository.UpdatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
            }

            return NoContent();
        }

        [HttpDelete("{pokemonId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeletePokemon(int pokemonId)
        {
            if (!await _pokemonRepository.PokemonExist(pokemonId))
            {
                return NotFound();
            }

            var reviewsToDelete = await _reviewRepository.GetReviewsOfAPokemon(pokemonId);
            var pokemonDelete = await _pokemonRepository.GetPokemon(pokemonId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when deleting reviews");
            }

            if (!await _pokemonRepository.DeletePokemon(pokemonDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the category");
            }

            return NoContent();

        }
    }
}
