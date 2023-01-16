using Core;

namespace Character.Combat
{
    public interface ICombatModule : IActionSender
    {
        ModuleType Flag { get; }
        ICombatObject CombatObject { get; }
    }
}
