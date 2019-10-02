namespace ES.Infrastructure.Http.Options
{
    public class RetryPolicyOptions
    {
        public int Count { get; set; }

        public int BackoffPower { get; set; } 
        
    }
}
