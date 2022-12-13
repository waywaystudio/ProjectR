using Core;
using UnityEngine;

namespace Common.Character.Operation.Combating.Entity
{
    public class HealEntity : BaseEntity, IHealProvider
    {
        public GameObject Provider => Cb.gameObject;
        public double CombatValue { get; set; }
        public float Critical { get; set; }

        public override bool IsReady => true;
        
        
        protected override void Awake()
        {
            base.Awake();

            SetEntity();
        }
        
        protected void SetEntity()
        {
            CombatValue = SkillData.BaseValue;
            Critical = 0.2f;
        }

        private void Reset()
        {
            flag = EntityType.Heal;
            SetEntity();
        }
    }
}
