using Polly;
using Polly.Extensions.Http;

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ES.Infrastructure.Serialization;
using ES.Infrastructure.Http.Options;

namespace ES.Infrastructure
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddSerialization(this IServiceCollection services)
        {
            services.AddScoped<IJsonSerializer, JsonNetSerializer>();

            return services;
        }
        public static IServiceCollection AddHttpPolicies(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("Policies:Graph");

            services.Configure<PolicyOptions>(configuration);

            var policyOptions = section.Get<PolicyOptions>();

            var policyRegistry = services.AddPolicyRegistry();

            policyRegistry.Add(
                PolicyName.HttpRetry,
                HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    policyOptions.HttpRetry.Count,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(policyOptions.HttpRetry.BackoffPower, retryAttempt))));

            policyRegistry.Add(
                PolicyName.HttpCircuitBreaker,
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .CircuitBreakerAsync(
                        handledEventsAllowedBeforeBreaking: policyOptions.HttpCircuitBreaker.ExceptionsAllowedBeforeBreaking,
                        durationOfBreak: policyOptions.HttpCircuitBreaker.DurationOfBreak));


            return services;
        }
    }
}
