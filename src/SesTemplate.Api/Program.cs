using SesTemplate.Api.Extensions;
using SesTemplate.Api.Factories;

var app = WebApplicationBuilderFactory.CreateWebApplication(args);
app.UseCors();
app.AddSwagger();
app.UseAuthorization();
app.UseMiddlewares();
app.MapControllers();
app.UseAngularFrontend();
app.Run();