using Core;

namespace Character.Combat
{
    public interface IModule : IActionSender
    {
        ModuleType Flag { get; }
    }

    public interface IOnStarted { ActionTable OnStarted { get; } }
    public interface IOnInterrupted { ActionTable OnInterrupted { get; } }
    public interface IOnCompleted { ActionTable OnCompleted { get; } }
}
