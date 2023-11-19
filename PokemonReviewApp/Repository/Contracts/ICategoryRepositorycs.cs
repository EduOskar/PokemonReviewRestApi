using PokemonReviewApp.Entity;

namespace PokemonReviewApp.Repository.Contracts
{
    public interface ICategoryRepositorycs
    {
        Task<ICollection<Category>> GetCategories();
        Task<Category> GetCategory(int id);
        Task<ICollection<Pokemon>> GetPokemonByCategory(int categoryId);
        Task<bool> CategoryExist(int Id);
        Task<bool> CreateCategory(Category category);

        Task<bool> Save();
    }
}
