using EcomerceOptimization.WebAPI.Configs;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomHttpClient();
builder.Services.RegisterServices(builder);
builder.Services.RegisterInfraestructure(builder.Configuration);
builder.Services.RegisterAuthentication(builder.Configuration);
builder.Services.RegisterApiVersioning();
builder.Services.AddSwaggerConfig();

var app = builder.Build();
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwaggerConfig(provider, builder.Configuration);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
