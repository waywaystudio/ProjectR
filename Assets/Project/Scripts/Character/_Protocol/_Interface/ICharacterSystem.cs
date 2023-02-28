using Character.Systems;

namespace Character
{
    using Graphic;

    public interface ICharacterSystem
    {
        SearchingSystem Searching { get; }
        CollidingSystem Colliding { get; }
        PathfindingSystem Pathfinding { get; }
        AnimationModel Animating { get; }
    }
}
