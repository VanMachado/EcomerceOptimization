using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace EcomerceOptimization.WebAPI.Configs
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<SwaggerDefaultValues>();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token no seguinte formato: Bearer <seu token>",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },

                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app, IApiVersionDescriptionProvider provider, IConfiguration config)
        {
            var swaggerDocVersion = string.Empty;
            var swaggerEndpointUrl = config.GetValue<string>("SwaggerSettings:EndpoiintUrl");
            var swaggerRoutePrefix = config.GetValue<string>("SwaggerSettings:RoutePrefix");
            var swaggerRouteTemplate = config.GetValue<string>("SwaggerSettings:RouteTemplate");

            app.UseSwagger();

            app.UseSwaggerUI(opt =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    swaggerDocVersion = description.GroupName;

                    opt.RoutePrefix = swaggerRoutePrefix;
                    opt.SwaggerEndpoint($"/{swaggerRoutePrefix}{swaggerEndpointUrl}", swaggerDocVersion);
                }
            });

            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint(swaggerEndpointUrl, swaggerDocVersion);
            });

            return app;
        }

        public class SwaggerDefaultValues : IOperationFilter
        {            
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var apiDescription = context.ApiDescription;
                operation.Deprecated |= apiDescription.IsDeprecated();

                foreach (var responseType in context.ApiDescription.SupportedResponseTypes)
                {
                    var responseKey = responseType.IsDefaultResponse ? "default" : responseType.StatusCode.ToString();
                    var response = operation.Responses[responseKey];

                    foreach(var contetType in response.Content.Keys)
                    {
                        if (responseType.ApiResponseFormats.All(x => x.MediaType != contetType))
                            response.Content.Remove(contetType);
                    }
                }

                if (operation.Parameters == null)
                    return;

                foreach (var parameter in operation.Parameters)
                {
                    var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
                    parameter.Description ??= description.ModelMetadata.Description;

                    if (parameter.Schema.Default == null && description.DefaultValue != null)
                    {
                        var json = JsonSerializer.Serialize(description.DefaultValue, description.ModelMetadata.ModelType);
                        parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
                    }

                    parameter.Required |= description.IsRequired;
                }                
            }
        }
    }
}
