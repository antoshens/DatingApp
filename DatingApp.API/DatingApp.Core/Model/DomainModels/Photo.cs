using DatingApp.Core.Model.DomainModels;

namespace DatingApp.Core.Model
{
    public partial class Photo : IEntity, IEntityAudit
    {
        public Photo()
        {

        }

        public Photo(string photoId, string url, bool isMain, User user)
        {
            PublicId = photoId;
            Url = url;
            IsMain = isMain;
            User = user;
        }
    }
}
