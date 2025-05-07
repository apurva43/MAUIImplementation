using Xunit;

public class HelloWorldTests
{
    [Fact]
    public void HelloWorld_ReturnsExpectedString()
    {
        var result = HelloWorld();
        Assert.Equal("Hello, World!", result);
    }

    private string HelloWorld()
    {
        return "Hello, World!";
    }
}