using Integration.BLModels;
using Integration.Mapping;
using Integration.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Integration.Repositories
{
    public interface IAppRepository
    {
        BLUser CreateUser(string userName, string firstName, string lastName, string email, string password, int countryId);
        void ChangePassword(string username, string newPassword);
        bool CheckUsernameExists(string username);
        bool CheckEmailExists(string email);
        BLUser GetUser(int id);
        void ConfirmEmail(string email, string securityToken);
        BLUser GetConfirmedUser(string username, string password);
        IEnumerable<BLVideo> GetAll();
        BLUser ViewProfile(string email);
    }
    public class AppRepository : IAppRepository
    {
        private readonly IntegrationContext _dbContext;
        public AppRepository(IntegrationContext dbContext)
        {
            _dbContext = dbContext;
        }
        public BLUser ViewProfile(string email) {
            User? dbUser = _dbContext.Users.FirstOrDefault(x => x.Email == email);
            var blUser = UserMapping.MapToBL(dbUser);
            return blUser;
        }
        public IEnumerable<BLVideo> GetAll()
        {
            var dbVideos = _dbContext.Videos;

            var blVideos = VideoMapping.MapToBL(dbVideos);

            return blVideos;
        }


        private static (byte[], string) GenerateSalt()
        {

            var salt = RandomNumberGenerator.GetBytes(128 / 8);
            var b64Salt = Convert.ToBase64String(salt);

            return (salt, b64Salt);
        }
        private static string CreateHash(string password, byte[] salt)
        {

            byte[] hash =
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);
            string b64Hash = Convert.ToBase64String(hash);

            return b64Hash;
        }
        private static string GenerateSecurityToken()
        {
            byte[] securityToken = RandomNumberGenerator.GetBytes(256 / 8);
            string b64SecToken = Convert.ToBase64String(securityToken);

            return b64SecToken;
        }
        public void ConfirmEmail(string email, string securityToken)
        {
            var userToConfirm = _dbContext.Users.FirstOrDefault(x =>
                x.Email == email &&
                x.SecurityToken == securityToken &&
                x.DeletedAt == null);
            userToConfirm.IsConfirmed = true;

            _dbContext.SaveChanges();
        }

        public BLUser CreateUser(string userName, string firstName, string lastName, string email, string password, int countryId)
        {
            (var salt, var b64Salt) = GenerateSalt();
            var b64Hash = CreateHash(password, salt);
            var b64SecToken = GenerateSecurityToken();

            var dbUser = new User
            {
                Username = userName,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PwdHash = b64Hash,
                PwdSalt = b64Salt,
                CreatedAt = DateTime.UtcNow,
                SecurityToken = b64SecToken,
                CountryOfResidence = _dbContext.Countries.FirstOrDefault(x => x.Id == countryId),
                CountryOfResidenceId = countryId
            };

            _dbContext.Users.Add(dbUser);

            _dbContext.SaveChanges();

            var bLUser = UserMapping.MapToBL(dbUser);

            return bLUser;
        }

        public BLUser GetUser(int id)
        {
            var target = _dbContext.Users.FirstOrDefault(x => x.Id == id);

            var blUser = UserMapping.MapToBL(target);

            return blUser;
        }
        public BLUser GetConfirmedUser(string username, string password)
        {
            var dbUser = _dbContext.Users.FirstOrDefault(
                x => x.Username == username &&
                x.IsConfirmed == true);


            if (dbUser == null)
            {

                return null;
            }

            var salt = Convert.FromBase64String(dbUser.PwdSalt);
            var b64hash = CreateHash(password, salt);

            if (dbUser.PwdHash != b64hash)
            {
                return null;
            }

            var blUser = UserMapping.MapToBL(dbUser);

            return blUser;
        }
        public bool CheckUsernameExists(string username)
    => _dbContext.Users.Any(x => x.Username == username && x.DeletedAt == null);

        public bool CheckEmailExists(string email)
            => _dbContext.Users.Any(x => x.Email == email && x.DeletedAt == null);

        public void ChangePassword(string username, string newPassword)
        {
            var userToChangePassword = _dbContext.Users.FirstOrDefault(x =>
                x.Username == username &&
                x.DeletedAt == null);

            (var salt, var b64Salt) = GenerateSalt();

            var b64Hash = CreateHash(newPassword, salt);

            userToChangePassword.PwdHash = b64Hash;
            userToChangePassword.PwdSalt = b64Salt;

            _dbContext.SaveChanges();
        }
    }
}
