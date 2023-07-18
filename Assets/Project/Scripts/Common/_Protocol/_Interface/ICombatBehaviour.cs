using Common.Characters.Behaviours;

namespace Common
{
    public interface ICombatBehaviour
    {
        StopBehaviour StopBehaviour { get; }
        RunBehaviour RunBehaviour { get; }
        StunBehaviour StunBehaviour { get; }
        KnockBackBehaviour KnockBackBehaviour { get; }
        DrawBehaviour DrawBehaviour { get; }
        DeadBehaviour DeadBehaviour { get; }
    }
}
