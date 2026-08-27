using Microsoft.Extensions.Configuration;

namespace AyahGraphApi.Infrastructure.Tests.Configuration;

public static class TestConfiguration
{
    public static IConfiguration Build()
    {
        return new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(
                "appsettings.test.json",
                optional: false)
            .Build();
    }
}