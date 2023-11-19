using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Entity;
using PokemonReviewApp.Repository.Contracts;

namespace PokemonReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _dataContext;

        public CountryRepository(DataContext dataContext )
        {
            _dataContext = dataContext;
        }


        public async Task<bool> CountryExist(int id)
        {
            var countryExist = await _dataContext.Countries.AnyAsync(c => c.Id == id);

            if (countryExist)
            {
                return countryExist;
            }

            throw new Exception("Country do not exist");
        }

        public async Task<bool> CreateCountry(Country country)
        {
            await _dataContext.AddAsync(country);

            await Save();

            return true;
        }

        public async Task<ICollection<Country>> GetCountries()
        {
            var countries = await _dataContext.Countries.ToListAsync();

            if (countries != null)
            {
                return countries;
            }

            throw new Exception("Error fetching countries");
        }

        public async Task<Country> GetCountry(int id)
        {
            var country = await _dataContext.Countries.FirstOrDefaultAsync(c => c.Id == id);

            if (country != null)
            {
                return country;
            }

            throw new Exception("There was no country matching the Id");
        }

        public Task<Country> GetCountryByOwner(int ownerId)
        {
            var countryByOwner = _dataContext.Owners.Where(
                o => o.Id == ownerId).Select(c => c.Country).FirstOrDefaultAsync();

            if (countryByOwner != null)
            {
                return countryByOwner!;
            }

            throw new Exception("There was no country matching the owner");

        }

        public async Task<ICollection<Owner>> GetOwnersFromACountry(int countryId)
        {
            var ownerFromCountry = await _dataContext.Owners.Where(o => o.Country!.Id == countryId).ToListAsync();

            if (ownerFromCountry != null)
            {
                return ownerFromCountry;
            }

            throw new Exception("There was no owners from that country");

        }

        public async Task<bool> Save()
        {
            var saved = await _dataContext.SaveChangesAsync();

            return saved > 0;
        }
    }
}
