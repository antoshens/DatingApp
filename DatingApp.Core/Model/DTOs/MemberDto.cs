namespace DatingApp.Core.Model.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Interests { get; set; }
        public string LookingFor { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Age { get; set; }
        public IEnumerable<PhotoDto> Photos { get; set; }
    }
}
