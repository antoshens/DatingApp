
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Core.Model
{
    public partial class Connection
    {
        [Key]
        public string ConnectionId {get; set;}

        public string UserName { get; set; }
    }
}
