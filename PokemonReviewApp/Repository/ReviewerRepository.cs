using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PokemonReviewApp.Data;
using PokemonReviewApp.Entity;
using PokemonReviewApp.Repository.Contracts;

namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _dataContext;

        public ReviewerRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Reviewer> GetReviewer(int reviewerId)
        {
            var reviewer = await _dataContext.Reviewers.Where(
                r => r.Id == reviewerId)
                .Include(e => e.Reviews)
                .SingleOrDefaultAsync();

            if (reviewer != null)
            {
                return reviewer;
            }

            throw new Exception("Couldn't request reviewer");

        }

        public async Task<ICollection<Reviewer>> GetReviewers()
        {
            var reviewers = await _dataContext.Reviewers.ToListAsync();

            if (!reviewers.IsNullOrEmpty())
            {
                return reviewers;
            }

            throw new Exception("Couldn't request reviewers");
        }

        public async Task<ICollection<Review>> GetReviewsByReviewer(int reviewerId)
        {
            var reviewer = await _dataContext.Reviews.Where(
                r => r.Reviewer!.Id == reviewerId)
                .ToListAsync();

            if (!reviewer.IsNullOrEmpty())
            {
                return reviewer;
            }

            throw new Exception("Couldn't request reviews  by reviewer");
        }

        public async Task<bool> ReviewerExist(int reviewerId)
        {
            var reviewerExist = await _dataContext.Reviewers.AnyAsync(rx => rx.Id == reviewerId);

            if (reviewerExist)
            {
                return reviewerExist;
            }

            return false;

        }
    }
}
