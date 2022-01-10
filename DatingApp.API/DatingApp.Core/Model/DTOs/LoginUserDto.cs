using System.ComponentModel.DataAnnotations;

namespace DatingApp.Core.Model.DTOs
{
    public class LoginUserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
