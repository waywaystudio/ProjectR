using Core;
using UnityEngine;

namespace Common.Character.Skills
{
    using Core;
    using Entity;

    public class AimShot : SkillAttribution, IDamageProvider
    {
        [SerializeField] private DamageEntity damageEntity;
        [SerializeField] private RangeEntity rangeEntity;
        [SerializeField] private CastingEntity castingEntity;
        [SerializeField] private TargetEntity targetEntity;

        public double Value => damageEntity.Value * damageEntity.AdditionalValue;
        public float Critical => damageEntity.CriticalChance;
        public float Hit => damageEntity.HitChance;
        
        public void OnDamage(ICombatAttribution combatInfo)
        {
            SetEntities(combatInfo);
            
            if (!IsReady) return;

            targetEntity.TargetList.ForEach(damageTaker => damageTaker.TakeDamage(this));
        }

        protected override void Awake()
        {
            base.Awake();
            
            damageEntity ??= GetComponent<DamageEntity>();
            rangeEntity ??= GetComponent<RangeEntity>();
            castingEntity ??= GetComponent<CastingEntity>();
        }
    }
}
