using Integration.Models;

namespace Integration.Repositories
{
    public interface ICountryRepository
    {
        IEnumerable<Country> GetAll();
    }
    public class CountryRepository : ICountryRepository
    {
        private readonly IntegrationContext _dbContext;
        public CountryRepository(IntegrationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Country> GetAll()
        {
            var dbCountries = _dbContext.Countries;

            return dbCountries;
        }
    }
}
