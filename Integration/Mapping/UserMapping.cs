using Integration.BLModels;
using Integration.Models;

namespace Integration.Mapping
{
    public class UserMapping
    {
        public static IEnumerable<BLUser> MapToBL(IEnumerable<User> users) =>
     users.Select(x => MapToBL(x));

        public static BLUser MapToBL(User user) =>
            new BLUser
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                IsConfirmed = user.IsConfirmed,
                CountryOfResidence = user.CountryOfResidence,
                SecurityToken= user.SecurityToken
            };

        public static IEnumerable<User> MapToDAL(IEnumerable<BLUser> blUsers)
            => blUsers.Select(x => MapToDAL(x));

        public static User MapToDAL(BLUser user) =>
            new User
            {
                Id = user.Id,
                CreatedAt = DateTime.Now,
                Username= user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                IsConfirmed = user.IsConfirmed,
                CountryOfResidence= user.CountryOfResidence
            };
    }
}
