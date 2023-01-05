using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class StatValue : Observable<float>
    {
        public StatValue() : this(0f) { }
        public StatValue(float value) => this.value = value;
        
        [SerializeField] protected StatCode statCode;

        public StatCode StatCode { get => statCode; set => statCode = value; }
    }

    [Serializable] public class PowerValue : StatValue
    {
        public PowerValue() : this(0f) { }
        public PowerValue(float value)
        {
            this.value = value;
            statCode   = StatCode.Power;
        }
    }

    [Serializable] public class CriticalValue : StatValue
    {
        public CriticalValue() : this(0f) { }
        public CriticalValue(float value)
        {
            this.value = value;
            statCode   = StatCode.Critical;
        }
    }
    
    [Serializable] public class HasteValue : StatValue
    {
        public HasteValue() : this(0f) { }
        public HasteValue(float value)
        {
            this.value = value;
            statCode   = StatCode.Haste;
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
    
    [Serializable] public class HitValue : StatValue
    {
        public HitValue() : this(0f) { }
        public HitValue(float value)
        {
            this.value = value;
            statCode   = StatCode.Hit;
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
            statCode   = StatCode.MaxHp;
        }
    }

    [Serializable] public class MaxResourceValue : StatValue
    {
        public MaxResourceValue() : this(0f) { }
        public MaxResourceValue(float value)
        {
            this.value = value;
            statCode   = StatCode.MaxResource;
        }
    }

    [Serializable] public class MoveSpeedValue : StatValue
    {
        public MoveSpeedValue() : this(0f) { }
        public MoveSpeedValue(float value)
        {
            this.value = value;
            statCode   = StatCode.MoveSpeed;
        }
    }

    [Serializable] public class ArmorValue : StatValue
    {
        public ArmorValue() : this(0f) { }
        public ArmorValue(float value)
        {
            this.value = value;
            statCode   = StatCode.Armor;
        }
    }

    [Serializable] public class EvadeValue : StatValue
    {
        public EvadeValue() : this(0f) { }
        public EvadeValue(float value)
        {
            this.value = value;
            statCode   = StatCode.Evade;
        }
    }

    [Serializable] public class ResistValue : StatValue
    {
        public ResistValue() : this(0f) { }
        public ResistValue(float value)
        {
            this.value = value;
            statCode   = StatCode.Resist;
        }
    }
}
