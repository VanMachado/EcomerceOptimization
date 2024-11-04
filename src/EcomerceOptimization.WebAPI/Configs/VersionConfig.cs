using Microsoft.AspNetCore.Mvc;

namespace EcomerceOptimization.WebAPI.Configs
{
    public static class VersionConfig
    {
        public static IServiceCollection RegisterApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(p =>
            {
                p.AssumeDefaultVersionWhenUnspecified = true;
                p.DefaultApiVersion = new ApiVersion(1, 0);
                p.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(p =>
            {
                p.GroupNameFormat = "'v'VVV";
                p.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}
