using UnityEngine;

namespace Core
{
    public interface ISkillInfo : IActionSender
    {
        // + ICombatProvider Provider { get; }
        // + DataIndex ActionCode { get; }

        Sprite Icon { get; }

        /* Casting Entity */
        bool HasCastingModule { get; }
        float CastingTime { get; }
        OldObservable<float> CastingProgress { get; }
        
        /* CoolTime Entity */
        bool HasCoolTimeModule { get; }
        float CoolTime { get; }
        OldObservable<float> RemainTime { get; }
    }

    public interface IOldStatusEffect : IActionSender
    {
        // + ICombatProvider Provider { get; }
        // + DataIndex ActionCode { get; }

        Sprite Icon { get; }
        bool IsBuff { get; }
        float Duration { get; }
        float CombatValue { get; }
        StatusEffectTable TargetTable { get; set; }
    }
}