using System;

namespace ExecAgent.Core.Models
{
    public sealed record ExecOutput(
        DateTimeOffset Timestamp,
        ExecStreamType Stream,
        string Data
    );
}
