using System;
using ES.Infrastructure.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ES.Test.Helpers
{
    public static class TestHelper
    {
        private static IConfiguration GetConfiguration(string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("Configuration/appsettings.json", optional: true)
                .Build();
        }
        public static IServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();

            var configuration = GetConfiguration(AppDomain.CurrentDomain.BaseDirectory);

            services.AddSingleton(configuration);

            services.AddTransient<IJsonSerializer,JsonNetSerializer> ();

            return services.BuildServiceProvider();
        }
    }
}
