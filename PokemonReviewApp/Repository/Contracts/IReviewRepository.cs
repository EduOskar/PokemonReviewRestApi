using PokemonReviewApp.Entity;

namespace PokemonReviewApp.Repository.Contracts
{
    public interface IReviewRepository
    {
        Task<ICollection<Review>> GetReviews();
        Task<Review> GetReview(int reviewId);
        Task<ICollection<Review>> GetReviewsOfAPokemon(int pokeId);
        Task<bool> ReviewExist(int reviewId);
        Task<bool> CreateReview(Review review);
        Task<bool> UpdateReview(Review review);
        Task<bool> DeleteReview(Review review);
        Task<bool> DeleteReviews(List<Review> reviews);
        Task<bool> Save();

    }
}
