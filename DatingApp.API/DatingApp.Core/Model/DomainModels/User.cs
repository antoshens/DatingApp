using DatingApp.Core.Model.DomainModels;
using DatingApp.Core.Extensions;
using System.Text;
using System.Security.Cryptography;

namespace DatingApp.Core.Model
{
    public partial class User : IEntity, IEntityAudit
    {
        public User()
        {

        }

        public User(string userName, byte[] passwordHash, byte[] passwordSalt, string email, string interests,
            string lookingFor, string city, string country, string publicPhotoId, string photoUrl,
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

            AddUserPhoto(publicPhotoId, photoUrl, true, this);

            if (!sex.HasValue || !Enum.IsDefined(typeof(UserSex), sex.Value))
            {
                throw new ArgumentException("Invalid value for user sex", nameof(Sex));
            }

            Sex = sex;
            IsDeleted = false;
        }

        public void AddUserPhoto(string publicId, string url, bool isMain, User user)
        {
            if (Photos == null) Photos = new List<Photo>();

            Photos.Add(new Photo(publicId, url, isMain, user));
        }

        public int? GetAge()
        {
            return BirthDate?.GetFullAge();
        }

        public void UpdateMainUserFields(string interests, string lookingFor, string city, string country,
            DateTime? birthDate = null, string? firstName = null, string? lastName = null, byte? sex = null)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Interests = interests;
            LookingFor = lookingFor;
            City = city;
            Country = country;
        }

        public void UpdateMainPhoto()
        {
            // TODO: Implement Cloudinary usage for storing photos
        }

        public void UpdatePassword(string prevoisPassword, string newPassword)
        {
            using var hmac = new HMACSHA512();

            var oldPassswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(prevoisPassword));
            var oldPasswordSalt = hmac.Key;

            if (PasswordHash != oldPassswordHash || PasswordSalt != oldPasswordSalt)
            {
                throw new ArgumentException("Old Password is invalid");
            }

            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
            PasswordSalt = hmac.Key;
        }

        public void UpdateEmailOrUserName(string newEmail, string newUserName)
        {
            if (!String.IsNullOrEmpty(newEmail))
            {
                Email = newEmail;
            }
            
            if (!String.IsNullOrEmpty(newUserName))
            {
                UserName = newUserName;
            }
        }

        public UserLike LikeUser(int likedUserId)
        {
            if (LikedUsers == null) LikedUsers = new List<UserLike>();

            var userLike = new UserLike(UserId, likedUserId);

            LikedUsers.Add(userLike);

            return userLike;
        }

        public void UnlikeUser(int unlikedUserId)
        {
            if (LikedUsers == null)
            {
                throw new ArgumentException("There is no users liked");
            }

            var likedUser = LikedUsers.FirstOrDefault(u => u.LikedUserId == unlikedUserId && !u.IsDeleted);

            if (likedUser is null)
            {
                throw new ArgumentNullException("No user found.");
            }

            likedUser.UnlikeUser();
        }
    }
}
