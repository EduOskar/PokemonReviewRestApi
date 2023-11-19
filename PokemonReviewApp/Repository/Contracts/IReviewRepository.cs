using PokemonReviewApp.Entity;

namespace PokemonReviewApp.Repository.Contracts
{
    public interface IReviewRepository
    {
        Task<ICollection<Review>> GetReviews();
        Task<Review> GetReview(int reviewId);
        Task<ICollection<Review>> GetReviewsOfAPokemon(int pokeId);
        Task<bool> reviewExist(int reviewId);

    }
}
