using System.ComponentModel.DataAnnotations;

namespace DatingApp.Core.Model.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 6)]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public DateTime BirthDate { get; set; }

        [EnumDataType(typeof(UserSex))]
        public byte Sex { get; set; }
        public string Interests { get; set; }
        public string LookingFor { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public byte[] MainPtotoHash { get; set; }
    }
}
