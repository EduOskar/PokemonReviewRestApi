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
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
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
            if(! await _reviewRepository.reviewExist(reviewId))
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


    }
}
