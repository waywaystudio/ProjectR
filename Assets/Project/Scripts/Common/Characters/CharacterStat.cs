using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Characters
{
    public class CharacterStat : MonoBehaviour
    {
        [SerializeField] private DataIndex baseStatCode;
        /* Add CombatClass Default Value */
        /* Add Equipments Value */

        public Dictionary<StatCode, StatTable> CharacterStatTable { get; } = new();

        /* Equipment, ClassBaseSpec */
        public void Add(string key, List<Stat> statList) => statList.ForEach(stat => Add(key, stat));
        public void Add(string key, Stat stat)
        {
            if (CharacterStatTable.ContainsKey(stat.StatType))
            {
                CharacterStatTable[stat.StatType].Add(key, stat);
            }
            else
            {
                var statTable = new StatTable();
                statTable.Add(key, stat);
                
                CharacterStatTable.Add(stat.StatType, statTable);
            }
        }

        /* OnEquipment Changed */
        public void Remove(string key, List<Stat> statList) => statList.ForEach(stat => Remove(key, stat));
        public void Remove(string key, Stat stat)
        {
            if (!CharacterStatTable.ContainsKey(stat.StatType)) return;
            
            CharacterStatTable[stat.StatType].Remove(key);
        }

        public float GetStatValue(StatCode statType)
        {
            if (!CharacterStatTable.ContainsKey(statType)) return 0;

            return CharacterStatTable[statType].Value;
        }

        public float Power => GetStatValue(StatCode.Power);
        public float Critical => GetStatValue(StatCode.Critical);
        public float Haste => GetStatValue(StatCode.Haste);
        public float MaxHp => GetStatValue(StatCode.MaxHp);
        public float MaxResource => GetStatValue(StatCode.MaxResource);
        public float MoveSpeed => GetStatValue(StatCode.MoveSpeed);
        public float Armor => GetStatValue(StatCode.Armor);

        public AliveValue Alive { get; } = new();
        public HpValue Hp { get; } = new();
        public ResourceValue Resource { get; } = new();
        public ShieldValue Shield { get; } = new();
        
        public StatusEffectTable DeBuffTable { get; } = new();
        public StatusEffectTable BuffTable { get; } = new();
    }
}
