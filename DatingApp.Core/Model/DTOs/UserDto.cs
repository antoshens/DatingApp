﻿using AutoMapper;
using DatingApp.Core.Model.AutoMapper;
using System.ComponentModel.DataAnnotations;

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
        public IEnumerable<PhotoDto> Photos { get; set; }

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
               s => s.MapFrom(x => x.Photos
                      .Select(_ => new Photo(_.PublicId, _.Url, _.IsMain))));
        }

        public void ConfigureMapTo(IMappingExpression<User, UserDto> mapping)
        {
            mapping.ForMember(x => x.Photos,
                s => s.MapFrom(x => x.Photos.Select(x => new PhotoDto
                    {
                        PublicId = x.PublicId,
                        Url = x.Url,
                        IsMain = x.IsMain,
                    })
                    .FirstOrDefault()));
        }
    }
}