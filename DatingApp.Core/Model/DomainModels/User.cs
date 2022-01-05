using DatingApp.Core.Model.DomainModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.Core.Model
{
    public partial class User : IEntity
    {
        [NotMapped]
        public int PrimaryKey => UserId;

        public bool IsDeleted { get; set; }

        public User(string userName, byte[] passwordHash, byte[] passwordSalt, string email,
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

            if (!sex.HasValue || !Enum.IsDefined(typeof(UserSex), sex.Value))
            {
                throw new ArgumentException("Invalid value for user sex", nameof(Sex));
            }

            Sex = sex;

            IsDeleted = false;
        }
    }
}
