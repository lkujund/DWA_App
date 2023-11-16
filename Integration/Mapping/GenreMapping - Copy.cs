using Integration.BLModels;
using Integration.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Integration.Mapping
{
    public class GenreMapping
    {
        public static IEnumerable<BLGenre> MapToBL(IEnumerable<Genre> genres) =>
     genres.Select(x => MapToBL(x));

        public static BLGenre MapToBL(Genre genre) =>
            new BLGenre
            {
                Id = genre.Id,
                Name = genre.Name,
                Description = genre.Description
            };

        public static IEnumerable<Genre> MapToDAL(IEnumerable<BLGenre> blGenres) 
            => blGenres.Select(x => MapToDAL(x));

        public static Genre MapToDAL(BLGenre genre) =>
            new Genre
            {
                Id = genre.Id ?? 0,
                Name = genre.Name,
                Description = genre.Description
            };
    }
}
