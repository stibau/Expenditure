using Microsoft.Extensions.Hosting;

namespace Hosting;

public class TestService(ITestClass testClass) : IHostedService
{
    private ITestClass TestClass { get; } = testClass;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("TestService is starting...");
        var a = 0;
        while (a < 50000)
        {
            a++;
            var currentResult = TestClass.GetResult(a);
            Console.WriteLine($"Iteration {currentResult}");
        }

        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("TestService is stopping...");
        await Task.CompletedTask;
    }
}