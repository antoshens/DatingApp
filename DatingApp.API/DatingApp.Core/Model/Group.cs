using System.ComponentModel.DataAnnotations;

namespace DatingApp.Core.Model
{
    public partial class Group
    {
        [Key]
        public string Name { get; set; }

        // Foregin Keys (FK)
        public ICollection<Connection> Connections { get; set; } // M:1 (FK) Connection.ConnectionId
    }
}
