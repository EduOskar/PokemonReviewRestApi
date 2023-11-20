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

        public async Task<bool> CreateReview(Review review)
        {
            await _dataContext.AddAsync(review);

            return await Save();
        }

        public async Task<bool> DeleteReview(Review review)
        {
            _dataContext.Remove(review);

            return await Save();
        }

        public async Task<bool> DeleteReviews(List<Review> reviews)
        {
            _dataContext.RemoveRange(reviews);

            return await Save();
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
                Where(r=>r.Pokemon!.Id == pokeId).ToListAsync();

            if (pokemonReview != null)
            {
                return pokemonReview;
            }

            return null!;
        }

        public async Task<bool> ReviewExist(int reviewId)
        {
            var reviewExist = await _dataContext.Reviews.AnyAsync(r => r.Id == reviewId);

            if (reviewExist)
            {
                return reviewExist;
            }

            return false;
        }

        public async Task<bool> Save()
        {
            var saved = await _dataContext.SaveChangesAsync();

            return saved > 0;
        }

        public async Task<bool> UpdateReview(Review review)
        {
            _dataContext.Update(review);

            return await Save();
        }
    }
}
