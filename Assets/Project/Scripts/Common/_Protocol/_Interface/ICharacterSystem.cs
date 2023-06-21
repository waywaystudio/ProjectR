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
        ActionTable OnHitEventTable { get; }
        void Idle();
        void Stop();
        void Run();
        void Play(string animationKey, float timeScale, System.Action callback);
    }
}
