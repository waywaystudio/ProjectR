using Common.Animation;
using Common.Systems;

namespace Common
{
    public interface ICharacterSystem
    {
        SearchingSystem Searching { get; }
        CollidingSystem Colliding { get; }
        PathfindingSystem Pathfinding { get; }
        AnimationModel Animating { get; }
    }
}
