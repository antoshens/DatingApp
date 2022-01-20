using AutoMapper;
using DatingApp.Core.Model.AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Core.Model.DTOs
{
    public class PhotoDto : IMapTo<Photo, PhotoDto>
    {
        [Required]
        public int PhotoId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string PublicId { get; set; }

        [Required]
        public string Url { get; set; }

        public bool IsMain { get; set; }

        public void ConfigureMapTo(IMappingExpression<Photo, PhotoDto> mapping)
        {
            mapping.ForMember(x => x.PhotoId, s => s.MapFrom(x => x.PhotoId));
            mapping.ForMember(x => x.UserId, s => s.MapFrom(x => x.User.UserId));
            mapping.ForMember(x => x.PublicId, s => s.MapFrom(x => x.PublicId));
            mapping.ForMember(x => x.Url, s => s.MapFrom(x => x.Url));
            mapping.ForMember(x => x.IsMain, s => s.MapFrom(x => x.IsMain));
        }
    }
}
