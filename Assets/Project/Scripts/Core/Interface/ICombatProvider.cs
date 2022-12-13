using UnityEngine;

namespace Core
{
    public interface IDamageProvider
    {
        GameObject Provider { get; }
        double CombatValue { get; }
        float Critical { get; }
        float Hit { get; }
    }

    public interface IHealProvider
    {
        GameObject Provider { get; }
        double CombatValue { get; }
        float Critical { get; }
    }

    public interface IExtraProvider
    {
        string Extra { get; }
    }
}
