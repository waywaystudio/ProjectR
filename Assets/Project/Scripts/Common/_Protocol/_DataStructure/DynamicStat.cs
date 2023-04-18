using System;
using UnityEngine;

namespace Common
{
    [Serializable] public class DynamicStat : FloatEvent { public StatTable StatTable { get; set; }}
    [Serializable] public class AliveValue : Observable<bool> { }
    
    [Serializable]
    public class HpValue : DynamicStat
    {
        public override float Value
        {
            get => value;
            set
            {
                if (Math.Abs(this.value - value) < 0.000001f) return;

                value      = Math.Max(value, 0);
                this.value = value;

                OnValueChanged?.Invoke(value);
            }
        }
    }
    
    [Serializable]
    public class ResourceValue : DynamicStat
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
    public class ShieldValue : DynamicStat
    {
        public override float Value
        {
            get => value;
            set
            {
                this.value = Mathf.Min(StatTable.Health * 1.0f, value);
                OnValueChanged?.Invoke(value);
            }
        }
    }
}