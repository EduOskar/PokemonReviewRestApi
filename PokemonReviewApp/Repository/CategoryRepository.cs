using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Entity;
using PokemonReviewApp.Repository.Contracts;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepositorycs
    {
        private readonly DataContext _dataContext;

        public CategoryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CategoryExist(int Id)
        {
            var categoryExist = await _dataContext.Categories.AnyAsync(c => c.Id == Id);

            if (categoryExist) 
            { 
              return true;
            }

            return false;

        }

        public async Task<ICollection<Category>> GetCategories()
        {
            var categories = await _dataContext.Categories.ToListAsync();

            if (categories != null)
            {
                return categories;
            }

            throw new Exception("Categories was not found");
        }

        public async Task<Category> GetCategory(int id)
        {
            var category = await _dataContext.Categories.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (category != null)
            {
                return category;
            }

            throw new Exception("Id didn't belong to any category");
        }

        public async Task<ICollection<Pokemon>> GetPokemonByCategory(int categoryId)
        {
            var pokemonsByCategory = await _dataContext.PokemonCategories
                .Where(pbc => pbc.CategoryId == categoryId)
                .Select(p => p.Pokemon)
                .ToListAsync();

            if (pokemonsByCategory != null)
            {
                return pokemonsByCategory!;
            }

            throw new Exception("Id didn't belong to any category");
        }
    }
}
