using System.Reflection;
using Microsoft.OpenApi.Models;
using Serilog;

namespace InfraServices;

public static class WebApplicationBuilderFactory
{
    public static WebApplicationBuilder CreateWebApplicationBuilder(string[] args)
    {
        // We create an empty builder with as few settings as possible pre-configured
        var builder = WebApplication.CreateEmptyBuilder(new WebApplicationOptions());

        // Now let's set up some Environment-related settings
        ConfigureEnvironment(builder.Environment);

        // Let's set up the application configuration
        ConfigureConfigurationProviders(builder.Configuration, builder.Environment, args);
        
        //Let's set up the logging
        ConfigureLogging(builder.Host, builder.Configuration);

        // Set up DI validation options
        ConfigureServiceValidation(builder.Host);

        // Let's configure some services here already. Note that
        // As long as we have not called build on the builder class, we can still add more services
        ConfigureServices(builder.Services);
        
        // Let's set up the WebHost
        ConfigureWebHost(builder.WebHost);

        return builder;
    }

    private static void ConfigureLogging(ConfigureHostBuilder builderHost, IConfigurationManager builderConfiguration)
    {
        // For now let's just add basic Console logging (note that this logger provider is added by default.
        // This uses Microsoft.Extensions.Logging, which is like the built-in logging system in .NET Core
        // Here we added it explicitly only for clarity)
        // builderLogging.ClearProviders();
        // builderLogging.AddConfiguration(configuration);
        // builderLogging.AddConsole();
        
        //Let's setup Serilog logging
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builderConfiguration).CreateLogger();
        builderHost.UseSerilog();
    }

    private static void ConfigureWebHost(ConfigureWebHostBuilder builderWebHost)
    {
        // Let's specify that we want to use Kestrel and configure it
        builderWebHost.UseKestrel((context, options) =>
        {
            options.Configure(context.Configuration.GetSection("Kestrel"));
        });
    }

    private static void ConfigureServiceValidation(ConfigureHostBuilder builderHost)
    {
        // The following sets two validation options on the host before building it.
        // They will ensure that the setup of the DI container is validated at the moment the host is built
        builderHost.UseDefaultServiceProvider(options =>
        {
            // This option will make sure build() throws if an error is detected with the dependencies that have been
            // set up in the DI container. E.g., a singleton that depends on a scoped service
            options.ValidateScopes = true;
            // This option will make sure build() throws if there is a missing dependency in the DI setup
            options.ValidateOnBuild = true;
        });
    }

    private static void ConfigureServices(IServiceCollection builderServices)
    {
        // Have all internal services to make routing and endpoint handling work
        builderServices.AddRouting();
    }

    private static void ConfigureEnvironment(IWebHostEnvironment environment)
    {
        // We set the Content Root Path explicitly (even though the default is already set to this variable)
        environment.ContentRootPath = Directory.GetCurrentDirectory();

        // We set the Environment Name to the one from either of two environment variables
        environment.EnvironmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                                      ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                                      ?? Environments.Production;

        // We explicitly set the Application Name as well (even though it is also set with a default value)
        environment.ApplicationName = Assembly.GetEntryAssembly()?.GetName().Name ?? "Application";
    }

    private static void ConfigureConfigurationProviders(IConfigurationManager configuration,
        IWebHostEnvironment builderEnvironment, string[] args)
    {
        // Clear all the configuration providers
        configuration.Sources.Clear();

        // Add two JSON file providers: 1) base appSettings.json and 2) appSettings.<environment>.json
        configuration.AddJsonFile("appSettings.json");
        configuration.AddJsonFile($"appSettings.{builderEnvironment.EnvironmentName}.json", true);

        // Add all the environment variables. They override the JSON file settings if needed
        configuration.AddEnvironmentVariables();

        // Add configuration from the command line
        configuration.AddCommandLine(args);
    }
}