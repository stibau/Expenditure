using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hosting;

public static class HostBuilderFactory
{
    public static IHostBuilder CreateGenericHostBuilder(string[]? args)
    {
        return new HostBuilder()
            // Sets the current directory as the directory from where any content is served (e.g. appSettings.ini)
            .UseContentRoot(Directory.GetCurrentDirectory())
            // This is used for configurations related to the host environment (logging, hosting settings, etc.).
            .ConfigureHostConfiguration(
                hostConfig =>
                {
                    // Environment variables starting with these prefixes will appear in the builderContext later on
                    hostConfig.AddEnvironmentVariables("DOTNET_");
                    hostConfig.AddEnvironmentVariables("ASPNETCORE_");

                    // This reads all environment variables, so thy can be added as key value pairs for example
                    // in the InMemoryCollection
                    var environmentVariables = Environment.GetEnvironmentVariables();
                    var country = (string?)(environmentVariables["COUNTRY"] ?? environmentVariables["country"]);
                    hostConfig.AddInMemoryCollection(
                    [
                        new KeyValuePair<string, string?>("country", country)
                    ]);

                    // Add command lin args; Format key=value
                    if (args != null)
                        hostConfig.AddCommandLine(args);
                })
            // This is used for configuring application-specific settings, like loading appSettings.json, environment variables, etc.
            .ConfigureAppConfiguration(args, "appSettings")
            // This is a method used to configure the dependency injection (DI) container.
            .ConfigureServices(
                (builderContext, services) =>
                {
                    // Here we can read whatever variable that was configured from the builder context
                    var factor = builderContext.Configuration.GetValue<int>("calculation:factor");

                    services.AddHostedService<TestService>();
                    services.AddSingleton<ITestClass, TestClass>(_ => new TestClass(factor));
                })
            // configures how the application handles dependency injection, allowing for better diagnostics and stricter validation,
            // helping developers ensure that services are configured correctly before they are used.
            .UseDefaultServiceProvider((_, options) =>
            {
                // This ensures that any invalid or misconfigured services are detected at build time.
                options.ValidateScopes = true;
                // This validates that scoped services are properly disposed of, preventing issues where
                // a scoped service is used incorrectly.
                options.ValidateOnBuild = true;
            });
    }

    private static IHostBuilder ConfigureAppConfiguration(
        this IHostBuilder hostBuilder,
        string[]? args,
        string baseSettingsFileName)
    {
        return hostBuilder.ConfigureAppConfiguration(
            (builderContext, appConfig) =>
            {
                // We get the environment from the HostingEnvironment (this is set from ASPNETCORE_ENVIRONMENT)
                var env = builderContext.HostingEnvironment;

                // Adds a JSON configuration provider to the builder. 
                appConfig
                    .AddJsonFile($"{baseSettingsFileName}.json")
                    .AddJsonFile($"{baseSettingsFileName}.{env.EnvironmentName}.json", true);

                //We add all the environment variables
                appConfig.AddEnvironmentVariables();

                //Add command line args format key=value
                if (args != null) appConfig.AddCommandLine(args);
            }
        );
    }
}