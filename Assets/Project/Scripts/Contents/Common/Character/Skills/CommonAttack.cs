using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Common.Character.Skills
{
    using Core;
    using Entity;
    
    public class CommonAttack : SkillAttribution, IDamageProvider
    {
        [SerializeField] private DamageEntity damageEntity;
        [SerializeField] private CoolTimeEntity coolTimeEntity;
        [SerializeField] private RangeEntity rangeEntity;
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
            coolTimeEntity ??= GetComponent<CoolTimeEntity>();
            rangeEntity ??= GetComponent<RangeEntity>();
            targetEntity ??= GetComponent<TargetEntity>();
        }
    }
}
