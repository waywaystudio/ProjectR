using UnityEngine;

namespace Core
{
    public interface IReady { bool IsReady { get; } }
    
    public interface ISkillInfo : IActionSender
    {
        // + ICombatProvider Provider { get; }
        // + IDCode ActionCode { get; }

        Sprite Icon { get; }

        /* Casting Entity */
        bool HasCastingModule { get; }
        float CastingTime { get; }
        Observable<float> CastingProgress { get; }
        
        /* CoolTime Entity */
        bool HasCoolTimeModule { get; }
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