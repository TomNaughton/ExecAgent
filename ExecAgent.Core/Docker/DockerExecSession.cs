using ExecAgent.Core.Abstractions;
using ExecAgent.Core.Models;
using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ExecAgent.Core.Docker
{
    public sealed class DockerExecSession : IExecSession
    {
        private readonly DockerClient _client;
        private readonly string _execId;
        private readonly bool _tty;

        public DockerExecSession(DockerClient client, string execId, bool tty)
        {
            _client = client;
            _execId = execId;
            _tty = tty;
        }

        public Task StartAsync(CancellationToken ct)
        {
            // No-op: start handled in StreamAsync
            return Task.CompletedTask;
        }

        public async IAsyncEnumerable<ExecOutput> StreamAsync([EnumeratorCancellation] CancellationToken ct)
        {
            using var stream = await _client.Exec.StartAndAttachContainerExecAsync(_execId, false, ct);
            var reader = new MultiplexedStreamReader(stream, _tty);

            await foreach (var output in reader.ReadOutputAsync(ct))
            {
                yield return output;
            }
        }

        public async Task<int?> GetExitCodeAsync()
        {
            var inspect = await _client.Exec.InspectContainerExecAsync(_execId);
            return (int?)inspect.ExitCode; // explicit cast from long -> int?
        }

        public Task ResizeAsync(int cols, int rows)
        {
            // This API no longer exists in Docker.DotNet 3.x
            // For now, we can no-op TTY resize or throw NotSupportedException
            return Task.CompletedTask;
        }

        public Task SendSignalAsync(string signal)
        {
            // Docker.DotNet 3.x removed KillContainerExecAsync
            // Cannot send signals for exec sessions directly anymore
            // Can no-op or throw NotSupportedException
            return Task.CompletedTask;
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }
    }
}
