using PokemonReviewApp.Entity;

namespace PokemonReviewApp.Repository.Contracts
{
    public interface IOwnerRepository
    {
        Task<ICollection<Owner>> GetOwners();
        Task<Owner> GetOwner(int ownerId);
        Task<ICollection<Owner>> GetOwnerOfPokemon(int pokeId);
        Task<ICollection<Pokemon>> GetPokemonByOwner(int ownerId);
        Task<bool> OwnerExist(int ownerId);
        Task<bool> CreateOwner(Owner owner);
        Task<bool> Save();
    }
}
