namespace DatingApp.Core.Model.DTOs
{
    public class GetMessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int SenderId { get; set; }
        public UserDto Sender { get; set; }
        public int RecepientId { get; set; }
        public UserDto Recepient { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateSent { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime? DateEdited { get; set; }
        public bool IsDeletedForSender { get; set; }
        public bool IsDeletedForRecepient { get; set; }
    }
}
