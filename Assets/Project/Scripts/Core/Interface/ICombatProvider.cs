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

        void CombatReport(ILog log);
    }
}
