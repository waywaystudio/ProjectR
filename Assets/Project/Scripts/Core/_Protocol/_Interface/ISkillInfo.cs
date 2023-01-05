using UnityEngine;

namespace Core
{
    public interface ISkillInfo : IActionSender
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
}