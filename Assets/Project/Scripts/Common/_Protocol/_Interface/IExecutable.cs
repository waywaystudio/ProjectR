using Common.Execution;

namespace Common
{
    public interface IExecutable : IOriginalProvider
    {
        // + ICombatProvider Provider { get; }
        ExecutionTable ExecutionTable { get; }
    }
}
