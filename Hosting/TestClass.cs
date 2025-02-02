namespace Hosting;

public class TestClass(int factor): ITestClass
{
    private int Factor { get; } = factor;

    public int GetResult(int input)
    {
        return input * Factor;
    }
    
}