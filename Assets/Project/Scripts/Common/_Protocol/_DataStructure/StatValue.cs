using System;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class StatValue : Observable<float>
    {
        [SerializeField] protected StatType statCode;
        
        public StatValue() : this(0f) { }
        public StatValue(float value) => this.value = value;

        public override float Value
        {
            get => value;
            set
            {
                if (Abs(value - this.value) < 0.0001f) return;
                
                this.value = value;
                OnValueChanged?.Invoke(value);
            }
        }
        
        public StatType StatCode { get => statCode; set => statCode = value; }

        private static float Abs(float number) =>
            number >= 0
                ? number
                : number * -1.0f;
    }

    [Serializable] public class PowerValue : StatValue
    {
        public PowerValue() : this(0f) { }
        public PowerValue(float value)
        {
            this.value = value;
            statCode   = StatType.Power;
        }
    }

    [Serializable] public class CriticalValue : StatValue
    {
        public CriticalValue() : this(0f) { }
        public CriticalValue(float value)
        {
            this.value = value;
            statCode   = StatType.CriticalChance;
        }
    }
    
    [Serializable] public class HasteValue : StatValue
    {
        public HasteValue() : this(0f) { }
        public HasteValue(float value)
        {
            this.value = value;
            statCode   = StatType.Haste;
        }
        
        public override float Value
        {
            get => value;
            set
            {
                this.value = Mathf.Max(0f, value);
                OnValueChanged?.Invoke(value);
            }
        }
    }

    [Serializable] public class MaxHpValue : StatValue
    {
        public MaxHpValue() : this(0f) { }
        public MaxHpValue(float value)
        {
            this.value = value;
            statCode   = StatType.MaxHp;
        }
    }

    [Serializable] public class MaxResourceValue : StatValue
    {
        public MaxResourceValue() : this(0f) { }
        public MaxResourceValue(float value)
        {
            this.value = value;
            statCode   = StatType.MaxResource;
        }
    }

    [Serializable] public class MoveSpeedValue : StatValue
    {
        public MoveSpeedValue() : this(0f) { }
        public MoveSpeedValue(float value)
        {
            this.value = value;
            statCode   = StatType.MoveSpeed;
        }
    }

    [Serializable] public class ArmorValue : StatValue
    {
        public ArmorValue() : this(0f) { }
        public ArmorValue(float value)
        {
            this.value = value;
            statCode   = StatType.Armor;
        }
    }
}