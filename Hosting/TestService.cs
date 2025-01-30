using Microsoft.Extensions.Hosting;

namespace Hosting;

public class TestService : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("TestService is starting...");
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("TestService is stopping...");
        await Task.CompletedTask;
    }
}