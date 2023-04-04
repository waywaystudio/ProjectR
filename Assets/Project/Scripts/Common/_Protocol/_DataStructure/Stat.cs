using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public enum StatApplyType
    {
        None = 0,
        Plus,
        Minus,
            
        /// <summary>
        /// 5% 증가 일 경우, value 값을 "5"로 작성. 0.05, 1.05 아님.
        /// </summary>
        PercentIncrease,
            
        /// <summary>
        /// 5% 감소 일 경우, value 값을 "5"로 작성. 0.05, 0.95, -5, -0.05 등등 아님.
        /// </summary>
        PercentDecrease
    }

    [Serializable]
    public class Stat
    {
        [SerializeField] private StatCode statType;
        [SerializeField] private StatApplyType applyType;
        [SerializeField] private float value;

        public StatCode StatType => statType;
        public StatApplyType ApplyType => applyType;
        public float Value => value;

        public Stat(StatCode statType, StatApplyType applyType, float value)
        {
            this.statType  = statType;
            this.applyType = applyType;
            this.value     = value;
        }
    }

    // [Serializable] public class PowerStat : Stat { }
    // [Serializable] public class HealthStat : Stat { }
    // [Serializable] public class CriticalStat : Stat { }
    // [Serializable] public class HasteStat : Stat { }
    // [Serializable] public class MoveSpeedStat : Stat { }
    // [Serializable] public class ArmorStat : Stat { }
    // [Serializable] public class MaxHpStat : Stat { }
    // [Serializable] public class MaxResourceStat : Stat { }
    // [Serializable] public class MinWeaponStat : Stat { }
    // [Serializable] public class MaxWeaponStat : Stat { }

    [Serializable]
    public class Sword
    {
        // [SerializeField] private EquipSlotType slotType;
        // [SerializeField] private int enchantLevel;
        // [SerializeField] private Sprite icon;
        [SerializeField] private List<Stat> statList = new();

        public List<Stat> StatList => statList;

        // [SerializeField] private PowerStat power;
        // [SerializeField] private HealthStat health;
        // [SerializeField] private CriticalStat critical;
        // [SerializeField] private HasteStat haste;
    }
}
