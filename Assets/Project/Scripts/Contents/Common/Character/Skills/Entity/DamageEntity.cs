using Common.Character.Skills.Core;
using UnityEngine;

namespace Common.Character.Skills.Entity
{
    public class DamageEntity : EntityAttribution, IDamageEntity
    {
        [SerializeField] private double value;
        [SerializeField] private float additionalValue = 1.0f;
        [SerializeField] private float criticalChance;
        [SerializeField] private float hitChance = 1.0f;
        [SerializeField] private float aggroCoefficient = 1.0f;
        
        public double Value { get => value; set => this.value = value; }
        public float AdditionalValue { get => additionalValue; set => additionalValue = value; }
        public float CriticalChance { get => criticalChance; set => criticalChance = value; }
        public float HitChance { get => hitChance; set => hitChance = value; }
        public float AdditionalAggro { get => aggroCoefficient; set => aggroCoefficient = value; }

        private void Awake()
        {
            Flag = EntityType.Damage;
        }

#if UNITY_EDITOR
        protected override void OnEditorInitialize()
        {
            base.OnEditorInitialize();
            
            Flag = EntityType.Damage;
        }
#endif
    }
}
