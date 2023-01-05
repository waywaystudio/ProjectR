using System;
using UnityEngine;

namespace Core
{
    [Serializable] public class DynamicStatValue : Observable<float> { public StatTable StatTable { get; set; }}
    [Serializable] public class AliveValue : Observable<bool> { }
    [Serializable]
    public class HpValue : DynamicStatValue
    {
        public override float Value
        {
            get => value;
            set
            {
                this.value = Mathf.Min(StatTable.MaxHp, value);
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
                this.value = Mathf.Min(StatTable.MaxResource, value);
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