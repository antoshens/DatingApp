using AutoMapper;
using DatingApp.Core.Model.AutoMapper;

namespace DatingApp.Core.Model.DTOs
{
    public class GetMessageDto : IMapTo<Message, GetMessageDto>
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int SenderId { get; set; }
        public int RecepientId { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateSent { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime? DateEdited { get; set; }
        public bool IsDeletedForSender { get; set; }
        public bool IsDeletedForRecepient { get; set; }

        public void ConfigureMapTo(IMappingExpression<Message, GetMessageDto> mapping)
        {
            mapping.ForMember(x => x.Id, s => s.MapFrom(m => m.MessageId));
        }
    }
}
