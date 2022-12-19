using UnityEngine;

namespace Core
{
    public interface ICombatProvider
    {
        int ID { get; }
        string Name { get; }

        GameObject Provider { get; }
        string ProviderName { get; }
        float CombatPower { get; }
        float Critical { get; }
        float Haste { get; }
        float Hit { get; }
    }

    public interface IStatusEffectProvider
    {
        int ID { get; }
        
        GameObject Provider { get; }
        string ProviderName { get; }
        double CombatValue { get; }
        float Critical { get; }
        float Haste { get; }
        float Hit { get; }
    }
}
