using PokemonReviewApp.Entity;

namespace PokemonReviewApp.Repository.Contracts
{
    public interface ICountryRepository
    {
        Task<ICollection<Country>> GetCountries();
        Task<Country> GetCountry(int id);
        Task<Country> GetCountryByOwner(int ownerId);
        Task<ICollection<Owner>> GetOwnersFromACountry(int countryId);
        Task<bool> CountryExist(int id);
    }
}
