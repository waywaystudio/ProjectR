using UnityEngine;

namespace Core
{
    public interface ISkill : IActionSender
    {
        // + ICombatProvider Provider { get; }
        // + IDCode ActionCode { get; }

        Sprite Icon { get; }

        /* Casting Entity */
        bool HasCastingEntity { get; }
        float CastingTime { get; }
        float CastingProgress { get; }
        
        /* CoolTime Entity */
        bool HasCoolTimeEntity { get; }
        float CoolTime { get; }
        Observable<float> RemainTime { get; }
    }

    public interface IStatusEffect : IActionSender
    {
        // + ICombatProvider Provider { get; }
        // + IDCode ActionCode { get; }

        Sprite Icon { get; }
        bool IsBuff { get; }
        float Duration { get; }
        float CombatValue { get; }
        StatusEffectTable TargetTable { get; set; }
    }
}