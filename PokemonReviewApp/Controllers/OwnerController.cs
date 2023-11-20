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
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, ICountryRepository countryRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<Owner>>> GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(await _ownerRepository.GetOwners());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owners);
        }

        [HttpGet("{ownerId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Owner>> GetOwner(int ownerId)
        {
            if (!await _ownerRepository.OwnerExist(ownerId))
            {
                return NotFound();
            }

            var owner = _mapper.Map<OwnerDto>(await _ownerRepository.GetOwner(ownerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owner);
        }

        [HttpGet("{ownerId:int}/pokemon")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Owner>> GetPokemonByOwner(int ownerId) 
        {
            if (!await _ownerRepository.OwnerExist(ownerId))
            {
                return NotFound();
            }

            var pokemonByOwner = _mapper.Map<List<PokemonDto>>(
                await _ownerRepository.GetPokemonByOwner(ownerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemonByOwner);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto ownerCreate)
        {
            if (ownerCreate == null)
            {
                return BadRequest(ModelState);
            }

            var owners = await _ownerRepository.GetOwners();
            var ownerNames = owners
                .Where(o => o.LastName?.Trim().ToUpper() == ownerCreate.LastName?.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (ownerNames != null)
            {
                ModelState.AddModelError("", "Owner already exist");
            }

            var ownerMap = _mapper.Map<Owner>(ownerCreate);

            ownerMap.Country = await _countryRepository.GetCountry(countryId);

            if (! await _ownerRepository.CreateOwner(ownerMap)) 
            {
                ModelState.AddModelError("", "Something went wrong while saving");
            }

            return Ok("Succesfully Created");
        }

        [HttpPut("{ownerId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateOwner(int ownerId, [FromBody] OwnerDto updatedOwner)
        {
            if (updatedOwner == null)
            {
                return BadRequest(ModelState);
            }

            if (ownerId != updatedOwner.Id)
            {
                return BadRequest(ModelState);
            }

            if (! await _ownerRepository.OwnerExist(ownerId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var ownerMap = _mapper.Map<Owner>(updatedOwner);

            if (! await _ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
            }

            return NoContent();
        }

        [HttpDelete("{ownerId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteOwner(int ownerId)
        {
            if (!await _ownerRepository.OwnerExist(ownerId))
            {
                return NotFound();
            }

            var ownerDelete = await _ownerRepository.GetOwner(ownerId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _ownerRepository.DeleteOwner(ownerDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the category");
            }

            return NoContent();

        }
    }
}
