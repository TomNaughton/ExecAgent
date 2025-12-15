using ExecAgent.Core.Models;
using Docker.DotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExecAgent.Core.Docker
{
    public sealed class MultiplexedStreamReader
    {
        private readonly MultiplexedStream _stream;
        private readonly bool _tty;

        public MultiplexedStreamReader(MultiplexedStream stream, bool tty)
        {
            _stream = stream;
            _tty = tty;
        }

        public async IAsyncEnumerable<ExecOutput> ReadOutputAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct)
        {
            using var stdout = new MemoryStream();
            using var stderr = new MemoryStream();

            // Pass stdin as null explicitly
            await _stream.CopyOutputToAsync(stdout, stderr, null, ct);

            // Reset positions
            stdout.Position = 0;
            stderr.Position = 0;

            if (stdout.Length > 0)
            {
                using var reader = new StreamReader(stdout, Encoding.UTF8);
                string content = await reader.ReadToEndAsync();
                yield return new ExecOutput(DateTimeOffset.UtcNow, ExecStreamType.StdOut, content);
            }

            if (stderr.Length > 0)
            {
                using var reader = new StreamReader(stderr, Encoding.UTF8);
                string content = await reader.ReadToEndAsync();
                yield return new ExecOutput(DateTimeOffset.UtcNow, ExecStreamType.StdErr, content);
            }
        }
    }
}
