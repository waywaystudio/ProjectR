using Core;
using UnityEngine;

namespace Common.Character.Skills.Core
{
    public class BaseSkill : MonoBehaviour
    {
        [SerializeField] protected int id;
        [SerializeField] protected string skillName;
        [SerializeField] protected int priority;
        [SerializeField] protected string motionType;

        public virtual bool IsReady { get; } = false;
        
        public virtual double Value { get; protected set; } = 1f;
        public virtual float Multiplier { get; protected set; } = 1f;
        public virtual float Critical { get; protected set; } = 5.0f;
        public virtual float HitChance { get; protected set; } = float.NegativeInfinity;
        public virtual float AggroMultiplier { get; protected set; } = 1f;

        protected virtual void Initialize()
        {}
        
        // [SerializeField] private float castingTime;
        // public virtual string ExtraStatus { get; protected set; } = string.Empty;
        
        // [SerializeField] protected int targetCount;
        // [SerializeField] protected string targetLayer;

        // IAbleToBattle
        // void TakeDamage(IDamageInfo damageInfo);
        // void TakeHeal(IHealInfo healInfo)
        // void TakeStatus(IStatusInfo statusInfo);

        // IRequiredTarget
        // void ActiveSkill(GameObject target, ICombat combatInfo);
        
        // IRequiredMultiTarget
        // void ActiveSkill(List<GameObject> targetList, ICombat combatInfo);
        
        // IRequiredPosition
        // void ActiveSkill(Vector3 position, ICombat combatInfo);

        // IHasCasting - Type을 나눌까...{ Casting, Channeling }
        // float CastingTime;
        // float RemainCastingTime;
        // bool IsCastingOn;
        // Action OnCastingStart;
        // Action OnCastingFinished;
        // void StartCasting();
        // void StopCasting();
        // void EndCasting();
        
        // IHasChanneling
        // float ChannelingTime;
        // float ChannelTerm; // 0.0 ~ 1.0
        // float RemainChannelingTime;
        // bool IsChannelingFinished;
        // Action OnChannelingStart;
        // Action OnChannelingFinished;
        // void StartChanneling();
        // void StopChanneling();
        // void EndChanneling();
        // Casting 과 Channeling 은 같은 오브젝트내에 할당 불가
        
        // ICombatInfo- 주체만 보자.
        // 공격력 값 : 스탯 * (스킬계수 + @)
        // 클래스 (어그로 환산율)
        // 치명타율 : 스탯 * (스킬계수 + @)
        // 주체가 공격한 방향 : 플레이어.forward
        // 거리 : 플레이어.position
        // 명중률 : 스탯 * (스킬계수 + @)
        // 상태이상 : 스킬특성
        
        // SKill마다 ICombatInfo를 가지고 있고,
        // CharacterBehavior에서 GiveDamage를 할 때,
        // Skill에 ICombatInfo값을 채워주고 타겟에게 넘기자.  
    }
}
