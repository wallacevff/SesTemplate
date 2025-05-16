using SesTemplate.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseCors();
app.AddSwagger();
app.UseAuthorization();
app.UseMiddlewares();
app.MapControllers();
app.UseAngularFrontend();
app.Run();