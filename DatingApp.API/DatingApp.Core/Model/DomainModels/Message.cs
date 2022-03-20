namespace DatingApp.Core.Model
{
    public partial class Message
    {
        public Message()
        {
        }

        public Message(int senderId, int recepientId, string content)
        {
            SenderId = senderId;
            RecipientId = recepientId;
            Content = content;

            IsDeletedForSender = false;
            IsDeletedForRecepient = false;
            IsRead = false;
            DateSent = DateTime.UtcNow;
        }

        public void MarkAsRead() => IsRead = true;
        public void MarkAsUnread() => IsRead = false;

        public Message Edit(string content)
        {
            Content = content;

            return this;
        }

        public void DeleteForSender() => IsDeletedForSender = true;
        public void DeleteForRecepient() => IsDeletedForRecepient = true;

        public void DeleteForEveryone()
        {
            DeleteForSender();
            DeleteForRecepient();
        }
    }
}
