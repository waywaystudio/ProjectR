using UnityEngine;

namespace Core
{
    public interface IDamageEntity
    {
        double Value { get; set; }
        float ValueCoefficient { get; set; }
        float CriticalChance { get; set; }
        float HitChance { get; set; }
        float AggroCoefficient { get; set; }
    }

    public interface IHasCoolDown
    {
        bool IsCoolOn { get; }
        float CoolTime { get; }
        float RemainCoolTime { get; }

        void DecreaseCoolDown(float tick);
    }

    public interface IHasRange
    {
        float Range { get; }
        float Distance { get; }
        bool IsInRange(Transform target);
    }
}
