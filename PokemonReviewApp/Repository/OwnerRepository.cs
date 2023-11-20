using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PokemonReviewApp.Data;
using PokemonReviewApp.Entity;
using PokemonReviewApp.Repository.Contracts;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _dataContext;

        public OwnerRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreateOwner(Owner owner)
        {
            await _dataContext.AddAsync(owner);

            return await Save();
        }

        public async Task<bool> DeleteOwner(Owner owner)
        {
            _dataContext.Remove(owner);

            return await Save();
        }

        public async Task<Owner> GetOwner(int ownerId)
        {
            var owner = await _dataContext.Owners.Where(o => o.Id == ownerId).FirstOrDefaultAsync();

            if (owner != null)
            {
                return owner;
            }

            throw new Exception("Owner was not found");
        }

        public async Task<ICollection<Owner>> GetOwnerOfPokemon(int pokeId)
        {
            var ownerByPokemon = await _dataContext.PokemonOwners.Where(
                po => po.Pokemon!.Id == pokeId)
                .Select(o => o.Owner)
                .ToListAsync();

            return ownerByPokemon!;

        }

        public async Task<ICollection<Owner>> GetOwners()
        {
            var owners = await _dataContext.Owners.ToListAsync();

            return owners;
        }

        public async Task<ICollection<Pokemon>> GetPokemonByOwner(int ownerId)
        {
            var pokemonByOwner = await _dataContext.PokemonOwners.Where(
                po => po.Owner!.Id == ownerId)
                .Select(o => o.Pokemon)
                .ToListAsync();

            return pokemonByOwner!;

        }

        public Task<bool> OwnerExist(int ownerId)
        {
            var ownerExist = _dataContext.Owners.AnyAsync(o => o.Id == ownerId);

            return ownerExist;
        }

        public async Task<bool> Save()
        {
            var saved = await _dataContext.SaveChangesAsync();

            return saved > 0;
        }

        public async Task<bool> UpdateOwner(Owner owner)
        {
            _dataContext.Update(owner);

            return await Save();
        }
    }
}
