using Common.Animation;
using Common.Systems;

namespace Common
{
    public interface ICharacterSystem : ICharacterAnimation
    {
        SearchingSystem Searching { get; }
        CollidingSystem Colliding { get; }
        PathfindingSystem Pathfinding { get; }
        // AnimationModel Animating { get; }
    }

    public interface ICharacterAnimation
    {
        AnimationModel Animating { get; }
    }
}
