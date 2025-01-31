using Microsoft.Extensions.Hosting;

namespace Hosting;

public class TestService : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("TestService is starting...");
        var a = 0;
        while (a < 50000)
        {
            a++;
            Console.WriteLine($"Iteration {a}");
        }

        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("TestService is stopping...");
        await Task.CompletedTask;
    }
}