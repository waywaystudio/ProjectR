using Core;
using UnityEngine;

namespace Common.Character.Operation.Combating.Entity
{
    public class DamageEntity : BaseEntity, IDamageProvider
    {
        public GameObject Provider => Cb.gameObject;
        public double CombatValue { get; set; } = 0d;
        public float AdditionalValue { get; set; }
        public float Critical { get; set; }
        public float Hit { get; set; }
        public float AdditionalAggro { get; set; }

        public override bool IsReady => true;

        protected void SetEntity()
        {
            // CombatValue = Cb.BaseStats.CombatValue
            AdditionalValue = SkillData.BaseValue;
            // CriticalChance = Cb.BaseStats.CriticalChance
            // HitChance = Cb.BaseStats.HitChance
            // AdditionalAggro = Cb.BaseStats.AdditionalAggro
        }

#if UNITY_EDITOR
        protected override void OnEditorInitialize()
        {
            flag = EntityType.Damage;

            SetEntity();
        }
#endif
    }
}
