using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

public static class ResiliencePolicy
{
    public static AsyncRetryPolicy RetryPolicy(ILogger logger) =>
        Policy.Handle<SqlException>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, (retryAttempt + 1))),
            onRetry: (exception, timeSpan, retryCount, context) =>
            {
                logger.LogWarning($"Try {retryCount} failed. Try again in {timeSpan.TotalSeconds} seconds.");
            });

    public static AsyncCircuitBreakerPolicy CircuitBreakerPolicy(ILogger logger) =>
        Policy.Handle<SqlException>()
            .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1),
            onBreak: (exception, timespan) =>
            {
                logger.LogWarning("Circuit opened for {TimeSpan}. Waiting for new tries.", timespan);
            },
            onReset: () =>
            {
                logger.LogInformation("Circuit closed. Trying again.");
            });
}
