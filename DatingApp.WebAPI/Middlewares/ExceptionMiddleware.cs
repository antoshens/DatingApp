using DatingApp.Core.Model.SystemResponse;
using Newtonsoft.Json;
using System.Net;

namespace DatingApp.WebAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            this._next = next;
            this._logger = logger;
            this._env = env;
        }

        public async Task InvokeAsync(HttpContext context)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ApiException(context.Response.StatusCode, "Internal Server Error");

                var jsonResponse = JsonConvert.SerializeObject(response);

                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
