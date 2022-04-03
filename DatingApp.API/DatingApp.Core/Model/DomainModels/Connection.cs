
namespace DatingApp.Core.Model
{
    public partial class Connection
    {
        public Connection()
        {

        }

        public Connection(string userName)
        {
            userName = userName;
        }

        public Connection(string connectionId, string userName)
        {
            ConnectionId = connectionId;
            UserName = userName;
        }
    }
}
