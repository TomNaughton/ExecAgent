using ExecAgent.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ExecAgent.Core.Abstractions
{
    public interface IExecutor
    {
        /// <summary>
        /// Creates a new execution session for the given request.
        /// </summary>
        Task<IExecSession> CreateSessionAsync(ExecRequest request, CancellationToken ct);
    }
}
