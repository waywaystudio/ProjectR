using Common.Animation;
using Common.Systems;

namespace Common
{
    public interface ICharacterSystem
    {
        SearchingSystem Searching { get; }
        CollidingSystem Colliding { get; }
        PathfindingSystem Pathfinding { get; }
    }

    public interface IAnimator
    {
        void Idle();
        void Run();
        void Dead();
        void Play(string animationKey, float timeScale, System.Action callback);
    }
}
