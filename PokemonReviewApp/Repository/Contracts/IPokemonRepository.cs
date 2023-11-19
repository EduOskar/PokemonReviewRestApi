using PokemonReviewApp.Entity;

namespace PokemonReviewApp.Repository.Contracts
{
    public interface IPokemonRepository
    {
        Task<ICollection<Pokemon>> GetPokemons();
        Task<Pokemon> GetPokemon(int id);
        Task<Pokemon> GetPokemon(string name);
        Task<decimal> GetPokemonRating(int pokeId);
        Task<bool> PokemonExist(int pokeId);
        Task<bool> CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        Task<bool> Save();
    }
}
