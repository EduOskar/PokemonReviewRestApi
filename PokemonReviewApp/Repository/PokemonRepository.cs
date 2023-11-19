using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Entity;
using PokemonReviewApp.Repository.Contracts;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _dataContext;

        public PokemonRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var PokemonOwnerEntity = await _dataContext.Owners.Where(o => o.Id == ownerId).FirstOrDefaultAsync();
            var category = await _dataContext.Categories.Where(c => c.Id == categoryId).FirstOrDefaultAsync();

            var pokemonOwner = new PokemonOwner()
            {
                Owner = PokemonOwnerEntity,
                Pokemon = pokemon,
            }; 

            _dataContext.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon,
            };

            _dataContext.Add(pokemonCategory);

            _dataContext.Add(pokemon);

            return true;
        }

        public async Task<Pokemon> GetPokemon(int pokeId)
        {
            var pokemon = await _dataContext.Pokemon.FindAsync(pokeId);

            if (pokemon != null)
            {
                return pokemon;
            }

            return null!;

        }

        public async Task<Pokemon> GetPokemon(string name)
        {
            var pokemon = await _dataContext.Pokemon.Where(p => p.Name == name).FirstOrDefaultAsync();

            if (pokemon != null)
            {
                return pokemon;
            }

            return null!;
        }

        public async Task<decimal> GetPokemonRating(int pokeId)
        {
            var review = await _dataContext.Reviews.Where(p => p.Pokemon!.Id == pokeId).ToListAsync();

            if (review.Count <= 0)
            {
                return 0;
            }

            return ((decimal)review.Sum(r => r.Rating) / review.Count);
        }

        public async Task<ICollection<Pokemon>> GetPokemons()
        {
            var pokemons = await _dataContext.Pokemon.OrderBy(p => p.Id).ToListAsync();

            return pokemons;
        }

        public async Task<bool> PokemonExist(int pokeId)
        {
            var pokemonExist = await _dataContext.Pokemon.AnyAsync(p => p.Id == pokeId);

            if (pokemonExist)
            {
                return pokemonExist;
            }

            return false!;
        }

        public async Task<bool> Save()
        {
            var saved = await _dataContext.SaveChangesAsync();
            return saved > 0;
        }
    }
}
