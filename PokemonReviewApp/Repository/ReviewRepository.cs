using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Entity;
using PokemonReviewApp.Repository.Contracts;

namespace PokemonReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _dataContext;

        public ReviewRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Review> GetReview(int reviewId)
        {
            var review = await _dataContext.Reviews.FindAsync(reviewId);

            if (review != null) 
            {
                return review;
            }

            return null!;
        }

        public async Task<ICollection<Review>> GetReviews()
        {
            var reviews = await _dataContext.Reviews.ToListAsync();

            if (reviews != null)
            {
                return reviews;
            }

            return null!;
        }

        public async Task<ICollection<Review>> GetReviewsOfAPokemon(int pokeId)
        {
            var pokemonReview = await _dataContext.Reviews.
                Where(r=>r.Pokemon.Id == pokeId).ToListAsync();

            if (pokemonReview != null)
            {
                return pokemonReview;
            }

            return null!;
        }

        public async Task<bool> reviewExist(int reviewId)
        {
            var reviewExist = await _dataContext.Reviews.AnyAsync(r => r.Id == reviewId);

            if (reviewExist)
            {
                return reviewExist;
            }

            return false;
        }
    }
}
