using Hosting;

try
{
    // We create WebApplication using our own in house WebApplicationFactory
    var app = WebApplicationFactory.CreateWebApplication(args);

    // Run the application
    await app.RunAsync();
}
catch (Exception exception)
{
    Console.WriteLine($"Host terminated unexpectedly: {exception.Message}");
    return 1;
}

return 0;