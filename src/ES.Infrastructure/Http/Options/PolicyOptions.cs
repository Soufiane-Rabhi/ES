namespace ES.Infrastructure.Http.Options
{
    public class PolicyOptions
    {
        public CircuitBreakerPolicyOptions HttpCircuitBreaker { get; set; }

        public RetryPolicyOptions HttpRetry { get; set; }
    }

    public static class PolicyName
    {
        public const string HttpCircuitBreaker = nameof(HttpCircuitBreaker);

        public const string HttpRetry = nameof(HttpRetry);
    }
}
