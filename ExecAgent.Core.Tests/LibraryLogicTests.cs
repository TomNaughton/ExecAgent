using Xunit;
using ExecAgent.Core.Models;

public class LibraryLogicTests
{
    [Fact]
    public void ExecRequest_SetsPropertiesCorrectly()
    {
        var request = new ExecRequest
        {
            Container = "test-container",  // required
            Command = new[] { "ls", "-la" }, // required
            Tty = true
        };

        Assert.Equal("test-container", request.Container);
        Assert.Equal(new[] { "ls", "-la" }, request.Command);
        Assert.True(request.Tty);
    }

    [Fact]
    public void ExecRequest_DefaultsAreCorrect()
    {
        var request = new ExecRequest
        {
            Container = "dummy-container",  // required
            Command = new string[] { }      // required
        };

        Assert.Equal("dummy-container", request.Container);
        Assert.Empty(request.Command);
        Assert.False(request.Tty);
    }
}
