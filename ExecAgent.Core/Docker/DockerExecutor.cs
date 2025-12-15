using ExecAgent.Core.Abstractions;
using ExecAgent.Core.Models;
using Docker.DotNet;
using Docker.DotNet.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExecAgent.Core.Docker
{
    public sealed class DockerExecutor : IExecutor
    {
        private readonly DockerClient _client;

        public DockerExecutor()
        {
            _client = new DockerClientConfiguration().CreateClient();
        }

        public async Task<IExecSession> CreateSessionAsync(ExecRequest request, CancellationToken ct)
        {
            var execParams = new ContainerExecCreateParameters
            {
                Cmd = request.Command.ToList(), // Convert to IList<string>
                AttachStdout = true,
                AttachStderr = true,
                Tty = request.Tty,
                User = request.User
            };

            // Correct call for Docker.DotNet 3.x:
            var execResponse = await _client.Exec.ExecCreateContainerAsync(
                request.Container, // <--- container name or ID
                execParams,
                ct
            );

            return new DockerExecSession(_client, execResponse.ID, request.Tty);
        }
    }
}
