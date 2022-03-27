namespace DatingApp.Core.Model
{
    public class BusinessResponse<T>
    {
        public T Data { get; set; }
        public bool Failed { get; set; } = false;
        public string FailedMessage { get; set; }
    }
}
