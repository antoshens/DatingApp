using DatingApp.Core.Model.DomainModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.Core.Model
{
    public partial class Message : IEntity
    {
        public int MessageId { get; set; }
        [NotMapped]
        public int PrimaryKey => MessageId;
        public int SenderId { get; set; }
        public User SenderUser { get; set; }
        public int RecipientId { get; set; }
        public User RecipientUser { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateSent { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime? DateEdited { get; set; }
        public bool IsDeletedForSender { get; set; }
        public bool IsDeletedForRecepient { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; }
    }
}
