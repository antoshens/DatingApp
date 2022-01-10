namespace DatingApp.Core.Model.SystemResponse
{
    public class ApiException
    {
        public ApiException(int statusCode, string message, string? details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details ?? String.Empty;
        }

        public int StatusCode { get; private set; }
        public string Message { get; private set; }
        public string? Details { get; private set; }
    }
}
