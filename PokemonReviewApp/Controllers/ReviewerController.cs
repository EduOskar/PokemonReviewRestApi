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
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<Reviewer>>> GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerDto>>(
                await _reviewerRepository.GetReviewers());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviewers);
        }

        [HttpGet("{reviewerId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Reviewer>> GetReviewer(int reviewerId)
        {
            if (! await _reviewerRepository.ReviewerExist(reviewerId))
            {
                return NotFound();
            }

            var reviewer = _mapper.Map<ReviewerDto>(await _reviewerRepository.GetReviewer(reviewerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviewer);
        }

        [HttpGet("{reviewerId:int}/reviewsByReviewer")]
        public async Task<ActionResult<Review>> GetReviewsByReviewer(int reviewerId)
        {

            if (!await _reviewerRepository.ReviewerExist(reviewerId))
            {
                return NotFound();
            }

            var reviews = _mapper.Map<List<ReviewDto>>(
              await  _reviewerRepository.GetReviewsByReviewer(reviewerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviews);
        }

    }
}
