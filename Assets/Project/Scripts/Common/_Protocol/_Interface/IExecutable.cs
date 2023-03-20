using Common.Execution;

namespace Common
{
    public interface IExecutable : IOriginalProvider
    {
        ExecutionTable ExecutionTable { get; }
    }
}
