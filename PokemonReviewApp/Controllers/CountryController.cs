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
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(
                await _countryRepository.GetCountries());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(countries);
        }
        [HttpGet("{countryId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Country>> GetCountry(int countryId)
        {

            if (await _countryRepository.CountryExist(countryId) == false)
            {
                return NotFound();
            }

            var country = _mapper.Map<CountryDto>(
                await _countryRepository.GetCountry(countryId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(country);
        }

        [HttpGet("/owners/{ownerId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Country>> GetCountryOfAnOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(
                await _countryRepository.GetCountryByOwner(ownerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> CreateCountry([FromBody] CountryDto countryCreate)
        {
            if (countryCreate == null)
            {
                return BadRequest(ModelState);
            }

            var country = await _countryRepository.GetCountries();

            var countryName = country
                .Where(c => c.Name!.Trim().ToUpper() == countryCreate.Name!.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (countryName != null)
            {
                ModelState.AddModelError("", "Country already exist");
                return StatusCode(422, ModelState);
            }

            var countryMap = _mapper.Map<Country>(countryCreate);

            if (! await _countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully Created");
        }

    }
}
