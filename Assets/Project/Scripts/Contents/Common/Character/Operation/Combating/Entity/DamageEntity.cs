using Core;
using UnityEngine;

namespace Common.Character.Operation.Combating.Entity
{
    public class DamageEntity : BaseEntity, IDamageProvider
    {
        [SerializeField] private double combatValue;
        [SerializeField] private float critical;
        [SerializeField] private float hit;
        
        public GameObject Provider => Cb.gameObject;
        public double CombatValue { get => combatValue; set => combatValue = value; }
        public float Critical { get => critical; set => critical = value; }
        public float Hit { get => hit; set => hit = value; }

        public override bool IsReady => true;


        protected override void Awake()
        {
            base.Awake();

            SetEntity();
        }

        public override void SetEntity()
        {
            CombatValue = SkillData.BaseValue;
            Critical = 0.2f;
            Hit = 0.95f;
        }

        private void Reset()
        {
            flag = EntityType.Damage;
            SetEntity();
        }
    }
}
