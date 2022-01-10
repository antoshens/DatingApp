using DatingApp.Infrastructure.IoC;
using DatingApp.WebAPI.Middlewares;
using DatingApp.WebAPI.Utils.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
RegisterServices(builder.Services, builder.Configuration);

builder.Services.AddAuthentication(builder.Configuration);

var hosturl = builder.Configuration.GetSection("HostUrl").Value;

builder.WebHost.UseKestrel().UseUrls(hosturl);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

//app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(_ => _.AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:4200")); // TODO: Use shared config file to store FE instance host address

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void RegisterServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    // Register services using DI container
    DependencyContainer.RegisterServices(services, configuration);
}
