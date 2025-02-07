namespace Hosting;

// ReSharper disable once ClassNeverInstantiated.Global
public class Program
{
    private static async Task<int> Main(string[] args)
    {
        try
        {
            var hostBuilder = HostBuilderFactory.CreateWebHostBuilder<Startup>(args);
            var host = hostBuilder.Build();
            await host.RunAsync();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return 1;
        }

        return 0;
    }
}