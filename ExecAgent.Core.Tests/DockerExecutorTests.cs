using ExecAgent.Core.Docker;
using ExecAgent.Core.Models;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class DockerExecutorTests
{
    [Fact]
    public async Task EchoCommand_ReturnsHelloUnitTest()
    {
        var executor = new DockerExecutor();

        var request = new ExecRequest
        {
            Container = "execagent-test", // Make sure this container exists
            Command = new[] { "echo", "Hello UnitTest" },
            Tty = true
        };

        var session = await executor.CreateSessionAsync(request, CancellationToken.None);

        var outputText = "";
        await foreach (var output in session.StreamAsync(CancellationToken.None))
        {
            outputText += output.Data;
        }

        var exitCode = await session.GetExitCodeAsync();

        Assert.Contains("Hello UnitTest", outputText);
        Assert.Equal(0, exitCode);
    }
}
