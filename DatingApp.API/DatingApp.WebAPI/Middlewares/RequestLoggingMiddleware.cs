namespace DatingApp.WebAPI.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public RequestLoggingMiddleware(RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            this._next = next;
            this._logger = logger;
            this._env = env;
        }

        public async Task InvokeAsync(HttpContext context, ICurrentUser currentuser)
        {
            if (context == null)
            {
                _logger.LogError($"{nameof(context)} is null!");

                if (_env.IsDevelopment())
                {
                    throw new ArgumentNullException(nameof(context));
                }
                else
                {
                    throw new ArgumentNullException("Internal Server Error");
                }
            }

            try
            {
                await _next(context);
            }
            finally
            {
                var currentUserId = currentuser.GetCurrentUserId();
                var method = context.Request.Method;
                var url = context.Request.Path;
                var user = !currentUserId.HasValue ? "Anonymous user" : $"User {currentUserId}";

                context.Request.EnableBuffering();
                context.Request.Body.Position = 0;
                var rawRequestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

                var message = $"{DateTime.UtcNow}: {user} requests {method} method by '{url}' with params {rawRequestBody}";
                _logger.LogDebug(message);
            }
        }
    }
}
