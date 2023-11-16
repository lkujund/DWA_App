using Integration.BLModels;
using Integration.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Integration.Mapping
{
    public class CountryMapping
    {
        public static IEnumerable<BLCountry> MapToBL(IEnumerable<Country> countries) =>
     countries.Select(x => MapToBL(x));

        public static BLCountry MapToBL(Country country) =>
            new BLCountry
            {
                Id = country.Id,
                Code= country.Code,
                Name = country.Name
            };

        public static IEnumerable<Country> MapToDAL(IEnumerable<BLCountry> blCountries) 
            => blCountries.Select(x => MapToDAL(x));

        public static Country MapToDAL(BLCountry country) =>
            new Country
            {
                Id = country.Id ?? 0,
                Code= country.Code,
                Name = country.Name
            };
    }
}
