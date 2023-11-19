using PokemonReviewApp.Entity;

namespace PokemonReviewApp.Repository.Contracts
{
    public interface IReviewerRepository
    {
        Task<ICollection<Reviewer>> GetReviewers();
        Task<Reviewer> GetReviewer(int reviewerId);
        Task<ICollection<Review>> GetReviewsByReviewer(int reviewerId);
        Task<bool> ReviewerExist(int reviewerId);
    }
}
