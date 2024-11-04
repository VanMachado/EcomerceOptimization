using Polly.Extensions.Http;
using Polly;
using EcomerceOptimization.Domain.Interfaces;
using EcomerceOptimization.Application.Service;

namespace EcomerceOptimization.WebAPI.Configs
{
    public static class InjectionConfig
    {        
        public static IServiceCollection RegisterServices(this IServiceCollection services, WebApplicationBuilder builder)
        {            
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IUserEcommerceService, UserEcommerceService>();            

            return services;
        }        

        public static IServiceCollection AddCustomHttpClient(this IServiceCollection service)
        {
            service.AddMemoryCache();
            service.AddLogging();

            service.AddHttpClient("ResilientHttpClient")
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri("https://localhost:7133/");
                })               
                .AddPolicyHandler(GetRetryPolicy(service.BuildServiceProvider().GetRequiredService<ILogger<IHttpClientFactory>>()))
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            return service;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ILogger logger)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (outcome, timespan, retryCount, context) =>
                    {                        
                        logger.LogWarning($"Try {retryCount} from {context.PolicyKey} failed. Waiting {timespan.TotalSeconds} seconds before next try.");
                    });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }                
    }
}
