using Microsoft.Extensions.Hosting;

namespace Hosting;

public class TestService : IHostedService
{
    public ITestClass TestClass { get; }

    public TestService(ITestClass testClass)
    {
        TestClass = testClass;
    }
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