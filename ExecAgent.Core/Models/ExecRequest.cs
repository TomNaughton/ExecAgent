using System;
using System.Collections.Generic;

namespace ExecAgent.Core.Models
{
    /// <summary>
    /// Represents a request to execute a command inside a container.
    /// Immutable once created.
    /// </summary>
    public sealed record ExecRequest
    {
        /// <summary>
        /// Name or ID of the container.
        /// </summary>
        public required string Container { get; init; }

        /// <summary>
        /// Command and arguments to execute.
        /// </summary>
        public required IReadOnlyList<string> Command { get; init; }

        /// <summary>
        /// Whether the execution requires a TTY.
        /// </summary>
        public bool Tty { get; init; } = false;

        /// <summary>
        /// Optional user to execute as inside the container.
        /// </summary>
        public string? User { get; init; }

        /// <summary>
        /// Optional environment variables.
        /// </summary>
        public IReadOnlyDictionary<string, string>? Environment { get; init; }

        /// <summary>
        /// Optional timeout for execution.
        /// </summary>
        public TimeSpan? Timeout { get; init; }
    }
}
