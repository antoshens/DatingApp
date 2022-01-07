namespace DatingApp.Core
{
    public partial class Photo
    {
        public Photo()
        {

        }

        public Photo(byte[] photoHash, bool isMain)
        {
            PhotoData = photoHash;
            IsMain = isMain;
        }
    }
}
