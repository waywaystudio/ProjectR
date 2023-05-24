using System;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class StatEntity
    {
        [SerializeField] protected StatType statType;
        [SerializeField] protected string statKey;
        [SerializeField] protected float value;

        public StatType StatType => statType;
        public string StatKey => statKey;

        public float Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValueChanged?.Invoke(this);
            }
        }
        public Action<StatEntity> OnValueChanged { get; set; }

        public StatEntity(StatType statType, string statKey, float value)
        {
            this.statType = statType;
            this.statKey  = statKey;
            this.value    = value;
        }
    }
}
