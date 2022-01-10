namespace DatingApp.Core.Model
{
    public partial class Photo
    {
        public Photo()
        {

        }

        public Photo(int photoId, string url, bool isMain)
        {
            PublicId = photoId;
            Url = url;
            IsMain = isMain;
        }
    }
}
