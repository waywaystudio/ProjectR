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

        public double Value => damageEntity.Value * damageEntity.AdditionalValue;
        public float Critical => damageEntity.CriticalChance;
        public float Hit => damageEntity.HitChance;

        protected override void Awake()
        {
            base.Awake();
            
            damageEntity ??= GetComponent<DamageEntity>();
            rangeEntity ??= GetComponent<RangeEntity>();
            castingEntity ??= GetComponent<CastingEntity>();
        }
    }
}
