using Core;
using UnityEngine;

namespace Common.Character.Operation.Combating.Entity
{
    public class HealEntity : BaseEntity, IHealProvider
    {
        [SerializeField] private double combatValue;
        
        public GameObject Provider => Cb.gameObject;
        public double CombatValue => combatValue;
        public float Critical => Cb.Critical.ResultToFloat;

        public override bool IsReady => true;
        
        
        protected override void Awake()
        {
            base.Awake();

            SetEntity();
        }
        
        public override void SetEntity()
        {
            combatValue = SkillData.BaseValue;
        }

        private void Reset()
        {
            flag = EntityType.Heal;
            SetEntity();
        }
    }
}
