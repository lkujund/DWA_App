using Integration.BLModels;
using Integration.Models;

namespace Integration.Mapping
{
    public class TagMapping
    {
        public static IEnumerable<BLTag> MapToBL(IEnumerable<Tag> tags) =>
tags.Select(x => MapToBL(x));

        public static BLTag MapToBL(Tag tag) =>
            new BLTag
            {
                Id = tag.Id,
                Name = tag.Name
            };

        public static IEnumerable<Tag> MapToDAL(IEnumerable<BLTag> blTags)
            => blTags.Select(x => MapToDAL(x));

        public static Tag MapToDAL(BLTag tag) =>
            new Tag
            {
                Id = tag.Id ?? 0,
                Name = tag.Name
            };
    }
}
