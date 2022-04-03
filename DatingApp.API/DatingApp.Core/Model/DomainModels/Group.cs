namespace DatingApp.Core.Model
{
    public partial class Group
    {
        public Group()
        {

        }

        public Group(string name)
        {
            name = name.Trim();
        }
    }
}
