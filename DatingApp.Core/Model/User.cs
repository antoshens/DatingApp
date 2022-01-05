namespace DatingApp.Core.Model
{
    public partial class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public byte? Sex { get; set; }
        public byte[] PasswordHash {get; set;}
        public byte[] PasswordSalt { get; set;}
    }
}
