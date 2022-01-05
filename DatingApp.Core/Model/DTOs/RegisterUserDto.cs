using System.ComponentModel.DataAnnotations;

namespace DatingApp.Core.Model.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public DateTime BirthDate { get; set; }

        [EnumDataType(typeof(UserSex))]
        public byte Sex { get; set; }
    }
}
