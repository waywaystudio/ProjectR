using System;
using UnityEngine;

namespace Core
{
    [Serializable] public class DynamicStatValue : FloatEvent { public StatTable StatTable { get; set; }}
    [Serializable] public class AliveValue : Observable<bool> { }
    
    [Serializable]
    public class HpValue : DynamicStatValue
    {
        public override float Value
        {
            get => value;
            set
            {
                if (Math.Abs(this.value - value) < 0.000001f) return;

                value      = Math.Clamp(value, 0, StatTable.MaxHp);
                this.value = value;

                OnValueChanged?.Invoke(value);
            }
        }
    }
    
    [Serializable]
    public class ResourceValue : DynamicStatValue
    {
        public override float Value
        {
            get => value;
            set
            {
                if (Math.Abs(this.value - value) < 0.000001f) return;

                value      = Math.Clamp(value, 0, StatTable.MaxResource);
                this.value = value;

                OnValueChanged?.Invoke(value);
            }
        }
    }
    
    [Serializable]
    public class ShieldValue : DynamicStatValue
    {
        public override float Value
        {
            get => value;
            set
            {
                this.value = Mathf.Min(StatTable.MaxHp * 0.1f, value);
                OnValueChanged?.Invoke(value);
            }
        }
    }
}