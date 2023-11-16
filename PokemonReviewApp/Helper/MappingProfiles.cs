using AutoMapper;
using PokemonReviewApp.Dtos;
using PokemonReviewApp.Entity;

namespace PokemonReviewApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon, PokemonDto>();
        }
    }
}
