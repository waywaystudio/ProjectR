using UnityEngine;

namespace Core
{
    public interface IReady { bool IsReady { get; } }
    public interface IOnActivated { ActionTable OnActivated { get; } }
    public interface IOnCompleted { ActionTable OnCompleted { get; } }
    public interface IOnCanceled { ActionTable OnCanceled { get; } }
    public interface IOnHit { ActionTable OnHit { get; } }

    // TODO. Graphic과 관련된 인터페이스로 이름 변경 및, 프로퍼티 변경 필요.
    public interface ISkill : IActionSender
    {
        // + ICombatProvider Provider { get; }
        // + IDCode ActionCode { get; }

        Sprite Icon { get; }
        int Priority { get; }

        /* Casting Entity */
        bool HasCastingModule { get; }
        float CastingTime { get; }
        float CastingProgress { get; }
        
        /* CoolTime Entity */
        bool HasCoolTimeModule { get; }
        float CoolTime { get; }
        Observable<float> RemainTime { get; }
        
        void Active();
        void Complete();
        void Hit();
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