using System;
using Core;
using UnityEngine;

namespace Common.Character.Skills.Core
{
    [Serializable]
    public class DamageEntity : IDamageEntity
    {
        public DamageEntity() : this(1f, 1f, 0.05f, 1f, 1f){}
        public DamageEntity(double value, float valueCoefficient, float criticalChance, float hitChance, float aggroCoefficient)
        {
            this.value = value;
            this.valueCoefficient = valueCoefficient;
            this.criticalChance = criticalChance;
            this.hitChance = hitChance;
            this.aggroCoefficient = aggroCoefficient;
        }
        
        [SerializeField] private double value;
        [SerializeField] private float valueCoefficient;
        [SerializeField] private float criticalChance;
        [SerializeField] private float hitChance;
        [SerializeField] private float aggroCoefficient;
        
        public double Value { get => value; set => this.value = value; }
        public float ValueCoefficient { get => valueCoefficient; set => valueCoefficient = value; }
        public float CriticalChance { get => criticalChance; set => criticalChance = value; }
        public float HitChance { get => hitChance; set => hitChance = value; }
        public float AggroCoefficient { get => aggroCoefficient; set => aggroCoefficient = value; }
    }
}
