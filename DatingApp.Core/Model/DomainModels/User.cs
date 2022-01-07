using DatingApp.Core.Model.DomainModels;
using DatingApp.Core.Extensions;

namespace DatingApp.Core.Model
{
    public partial class User : IEntity, IEntityAudit
    {
        public User()
        {

        }

        public User(string userName, byte[] passwordHash, byte[] passwordSalt, string email,
            string interests, string lookingFor, string city, string country, byte[] mainPhotoHash,
            DateTime? birthDate = null, string? firstName = null, string? lastName = null, byte? sex = null)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException(nameof(UserName));
            }

            if (passwordHash.Length == 0 || passwordSalt.Length == 0)
            {
                throw new ArgumentException("You should have a password", nameof(PasswordSalt));
            }

            UserName = userName;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            BirthDate = birthDate;
            Interests = interests;
            LookingFor = lookingFor;
            City = city;
            Country = country;

            AddUserPhoto(mainPhotoHash, true);

            if (!sex.HasValue || !Enum.IsDefined(typeof(UserSex), sex.Value))
            {
                throw new ArgumentException("Invalid value for user sex", nameof(Sex));
            }

            Sex = sex;
            IsDeleted = false;
        }

        public void AddUserPhoto(byte[] photoHash, bool isMain)
        {
            Photos.Add(new Photo(photoHash, isMain));
        }

        public int? GetAge()
        {
            return BirthDate?.GetFullAge();
        }
    }
}
