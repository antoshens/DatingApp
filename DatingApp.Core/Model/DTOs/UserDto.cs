using AutoMapper;
using DatingApp.Core.Model.AutoMapper;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DatingApp.Core.Model.DTOs
{
    public class UserDto : IMapFrom<UserDto, User>, IMapTo<User, UserDto>
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public string Interests { get; set; }

        [Required]
        public string LookingFor { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string MainPhotoData { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public DateTime BirthDate { get; set; }

        [EnumDataType(typeof(UserSex))]
        public byte Sex { get; set; }

        public void ConfigureMapFrom(IMappingExpression<UserDto, User> mapping)
        {
            mapping.ForMember(x => x.Photos,
                s => s.MapFrom(x => new List<Photo> {
                    new Photo(Encoding.UTF8.GetBytes(x.MainPhotoData), true)
                }));
        }

        public void ConfigureMapTo(IMappingExpression<User, UserDto> mapping)
        {
            mapping.ForMember(x => x.MainPhotoData,
                s => s.MapFrom(x => x.Photos.Where(p => p.IsMain)
                                .Select(x => x.PhotoData)
                                .FirstOrDefault()));
        }
    }
}
