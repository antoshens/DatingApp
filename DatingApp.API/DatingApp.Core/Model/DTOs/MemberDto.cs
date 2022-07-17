using AutoMapper;
using DatingApp.Core.Model.AutoMapper;

namespace DatingApp.Core.Model.DTOs
{
    public class MemberDto : IMapTo<User, MemberDto>
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Interests { get; set; }
        public LookingFor LookingFor { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Age { get; set; }
        public IEnumerable<PhotoDto> Photos { get; set; }

        public void ConfigureMapTo(IMappingExpression<User, MemberDto> mapping)
        { 
            mapping.ForMember(x => x.Login, s => s.MapFrom(x => x.Email));
            mapping.ForMember(x => x.FirstName, s => s.MapFrom(x => x.FirstName));
            mapping.ForMember(x => x.LastName, s => s.MapFrom(x => x.LastName));
            mapping.ForMember(x => x.Interests, s => s.MapFrom(x => x.Interests));
            mapping.ForMember(x => x.LookingFor, s => s.MapFrom(x => x.LookingFor));
            mapping.ForMember(x => x.Age, s => s.MapFrom(x => x.BirthDate.HasValue ? (DateTime.UtcNow.Date - x.BirthDate.Value.Date).TotalDays / 365.2425 : 0));

            mapping.ForMember(x => x.Photos,
                s => s.MapFrom(x => x.Photos.Select(x => new PhotoDto
                {
                    PhotoId = x.PhotoId,
                    PublicId = x.PublicId,
                    Url = x.Url,
                    IsMain = x.IsMain,
                    UserId = x.UserId
                })));
        }
    }
}
