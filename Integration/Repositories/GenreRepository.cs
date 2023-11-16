using Azure;
using Integration.BLModels;
using Integration.Mapping;
using Integration.Models;
using Microsoft.EntityFrameworkCore;

namespace Integration.Repositories
{
    public interface IGenreRepository
    {
        BLGenre AddGenre(BLGenre genre);
        BLGenre EditGenre(BLGenre genre);
        void DeleteGenre(BLGenre genre);
    }
    public class GenreRepository : IGenreRepository
    {
        private readonly IntegrationContext _dbContext;
        public GenreRepository(IntegrationContext dbContext)
        {
            _dbContext = dbContext;
        }
        public BLGenre AddGenre(BLGenre genre)
        {
            var dbGenre = GenreMapping.MapToDAL(genre);

            _dbContext.Genres.Add(dbGenre);

            _dbContext.SaveChanges();

            return genre;
        }

        public BLGenre EditGenre(BLGenre genre)
        {
            var target = _dbContext.Genres.FirstOrDefault(x => x.Id == genre.Id);

            target.Name = genre.Name;
            target.Description = genre.Description;

            _dbContext.SaveChanges();

            return genre;
        }

        public void DeleteGenre(BLGenre genre)
        {
            var target = _dbContext.Genres.FirstOrDefault(x => x.Id == genre.Id);
            _dbContext.Genres.Remove(target);
            _dbContext.SaveChanges();
        }
    }
}
