using Integration.BLModels;
using Integration.Mapping;
using Integration.Models;

namespace Integration.Repositories
{
    public interface ITagRepository
    {
        BLTag AddTag(BLTag tag);
        BLTag EditTag(BLTag tag);
        void DeleteTag(BLTag tag);
    }
    public class TagRepository : ITagRepository
    {
        private readonly IntegrationContext _dbContext;
        public TagRepository(IntegrationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public BLTag AddTag(BLTag tag)
        {
            var dbTag = TagMapping.MapToDAL(tag);

            _dbContext.Tags.Add(dbTag);

            _dbContext.SaveChanges();

            return tag;
        }

        public BLTag EditTag(BLTag tag)
        {
            var target = _dbContext.Tags.FirstOrDefault(x => x.Id == tag.Id);
            
            target.Name = tag.Name;

            _dbContext.SaveChanges();

            return tag;
        }

        public void DeleteTag(BLTag tag)
        {
            var target = _dbContext.Tags.FirstOrDefault(x => x.Id == tag.Id);
            _dbContext.Tags.Remove(target);
            _dbContext.SaveChanges();
        }
    }
}
