using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dtos;
using PokemonReviewApp.Entity;
using PokemonReviewApp.Repository;
using PokemonReviewApp.Repository.Contracts;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository, IPokemonRepository pokemonRepository, IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(
                await _reviewRepository.GetReviews());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviews);
        }
        [HttpGet("{reviewId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Review>> GetReview(int reviewId)
        {
            if(! await _reviewRepository.ReviewExist(reviewId))
            {
                return NotFound();
            }

            var review = _mapper.Map<ReviewDto>(
                await _reviewRepository.GetReview(reviewId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(review);
        }
        [HttpGet("{pokeId:int}/GetReviewsOfAPokemon")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Review>> GetReviewsOfAPokemon(int pokeId)
        {

            var pokemonReview = _mapper.Map<List<ReviewDto>>(
                await _reviewRepository.GetReviewsOfAPokemon(pokeId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemonReview);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> CreateReview(
            [FromQuery] int reviewerId, [FromQuery] int pokemonId, 
            [FromBody] ReviewDto reviewCreate)
        {
            if (reviewCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewMap = _mapper.Map<Review>(reviewCreate);

            reviewMap.Pokemon = await _pokemonRepository.GetPokemon(pokemonId);
            reviewMap.Reviewer = await _reviewerRepository.GetReviewer(reviewerId);

            if (! await _reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "There was an error saving");
            }

            return Ok("Successfully created");

        }

        [HttpPut("{reviewId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateReviewer(int reviewId, [FromBody] ReviewDto updateReview)
        {
            if (updateReview == null)
            {
                return BadRequest(ModelState);
            }

            if (reviewId != updateReview.Id)
            {
                return BadRequest(ModelState);
            }

            if (!await _reviewRepository.ReviewExist(reviewId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var reviewMap = _mapper.Map<Review>(updateReview);

            if (!await _reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
            }

            return NoContent();
        }

        [HttpDelete("{reviewId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteReview(int reviewId)
        {
            if (!await  _reviewRepository.ReviewExist(reviewId))
            {
                return NotFound();
            }

            var reviewDelete = await _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _reviewRepository.DeleteReview(reviewDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the category");
            }

            return NoContent();

        }


    }
}
