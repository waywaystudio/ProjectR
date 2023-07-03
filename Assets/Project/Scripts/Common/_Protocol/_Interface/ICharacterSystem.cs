using Common.Systems;

namespace Common
{
    public interface ICharacterSystem
    {
        PathfindingSystem Pathfinding { get; }
    }
}
