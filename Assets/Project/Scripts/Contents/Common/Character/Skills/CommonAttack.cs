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

        public double Value => damageEntity.Value * damageEntity.AdditionalValue;
        public float Critical => damageEntity.CriticalChance;
        public float Hit => damageEntity.HitChance;

        protected override void Awake()
        {
            base.Awake();

            damageEntity ??= GetComponent<DamageEntity>();
            coolTimeEntity ??= GetComponent<CoolTimeEntity>();
            rangeEntity ??= GetComponent<RangeEntity>();
        }
    }
}
