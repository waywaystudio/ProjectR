using Core;
using UnityEngine;

namespace Common.Character.Operation.Combating.Entity
{
    public class HealEntity : BaseEntity, IHealProvider
    {
        [SerializeField] private double combatValue;
        [SerializeField] private float critical;
        
        public GameObject Provider => Cb.gameObject;
        public double CombatValue { get => combatValue; set => combatValue = value; }
        public float Critical { get => critical; set => critical = value; }

        public override bool IsReady => true;
        
        
        protected override void Awake()
        {
            base.Awake();

            SetEntity();
        }
        
        public override void SetEntity()
        {
            combatValue = SkillData.BaseValue;
            critical = 0.2f;
        }

        private void Reset()
        {
            flag = EntityType.Heal;
            SetEntity();
        }
    }
}
