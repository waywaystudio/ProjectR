using Core;
using UnityEngine;

namespace Common.Character.Operation.Combating.Entity
{
    public class DamageEntity : BaseEntity, IDamageProvider
    {
        [SerializeField] private double combatValue;
        
        public GameObject Provider => Cb.gameObject;
        public double CombatValue { get => combatValue; set => combatValue = value; }
        public float Critical => Cb.Critical.Result;
        public float Hit => Cb.Hit.Result;

        public override bool IsReady => true;


        protected override void Awake()
        {
            base.Awake();

            SetEntity();
        }

        public override void SetEntity()
        {
            CombatValue = SkillData.BaseValue;
        }

        private void Reset()
        {
            flag = EntityType.Damage;
            SetEntity();
        }
    }
}
