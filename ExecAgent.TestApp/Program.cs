using ExecAgent.Core.Docker;
using ExecAgent.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExecAgent.TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting ExecAgent test...");

            // Replace with your container name
            string containerName = "execagent-test";

            var executor = new DockerExecutor();

            var request = new ExecRequest
            {
                Container = containerName,
                Command = new[] { "echo", "Hello ExecAgent" },
                Tty = false
            };

            try
            {
                var session = await executor.CreateSessionAsync(request, CancellationToken.None);

                Console.WriteLine("Streaming output:");
                await foreach (var output in session.StreamAsync(CancellationToken.None))
                {
                    Console.WriteLine($"{output.Stream}: {output.Data}");
                }

                var exitCode = await session.GetExitCodeAsync();
                Console.WriteLine($"Execution finished with exit code: {exitCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
