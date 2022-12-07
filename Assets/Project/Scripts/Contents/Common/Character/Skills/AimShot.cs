using System.Collections.Generic;
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

        public GameObject Provider => Cb.gameObject;
        public double Value => damageEntity.CombatValue * damageEntity.AdditionalValue;
        public float Critical => damageEntity.CriticalChance;
        public float Hit => damageEntity.HitChance;
        
        public override List<ICombatTaker> TargetList => targetEntity.TargetList;

        public override void Invoke()
        {
            TargetList.ForEach(target => target.TakeDamage(this));
        }

        protected override void Awake()
        {
            base.Awake();
            
            damageEntity ??= GetComponent<DamageEntity>();
            rangeEntity ??= GetComponent<RangeEntity>();
            castingEntity ??= GetComponent<CastingEntity>();
            targetEntity ??= GetComponent<TargetEntity>();
        }
    }
}
