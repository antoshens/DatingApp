
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Core.Model.DTOs
{
    public class LogedUserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
