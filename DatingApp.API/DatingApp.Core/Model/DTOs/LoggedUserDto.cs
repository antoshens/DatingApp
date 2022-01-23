
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Core.Model.DTOs
{
    public class LoggedUserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
