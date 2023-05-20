using System;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class Stat
    {
        [SerializeField] protected StatType statType;
        [SerializeField] protected string statKey;
        [SerializeField] protected float value;

        public StatType StatType => statType;
        public string StatKey => statKey;
        public float Value => value;
        
        public Stat(StatType statType, string statKey, float value)
        {
            this.statType = statType;
            this.statKey  = statKey;
            this.value    = value;
        }
    }
}
