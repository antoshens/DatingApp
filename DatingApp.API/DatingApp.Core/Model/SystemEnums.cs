namespace DatingApp.Core.Model
{
    public enum UserSex : byte
    {
        Unknown = 0,
        Male = 1,
        Female = 2,
        NonBinary = 3,
    }

    public enum MessageOrientedOption : byte
    {
        Sender = 1,
        Recipient = 2
    }

    public enum MessageDeletionOption : byte
    {
        ForSender = 1,
        ForRecepient = 2,
        ForEveryone = 3
    }
}
