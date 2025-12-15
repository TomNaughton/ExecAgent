using ExecAgent.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExecAgent.Core.Abstractions
{
    public interface IExecSession : IAsyncDisposable
    {
        /// <summary>
        /// Starts the execution session.
        /// </summary>
        Task StartAsync(CancellationToken ct);

        /// <summary>
        /// Streams output (stdout/stderr) as it occurs.
        /// </summary>
        IAsyncEnumerable<ExecOutput> StreamAsync(CancellationToken ct);

        /// <summary>
        /// Sends a signal to the running process.
        /// </summary>
        Task SendSignalAsync(string signal);

        /// <summary>
        /// Optionally resizes the TTY if TTY is enabled.
        /// </summary>
        Task ResizeAsync(int cols, int rows);

        /// <summary>
        /// Retrieves the exit code once execution is complete.
        /// </summary>
        Task<int?> GetExitCodeAsync();
    }
}
