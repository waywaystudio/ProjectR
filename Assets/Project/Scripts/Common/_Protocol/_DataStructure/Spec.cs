using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [Serializable] 
    public class Spec
    {
        [SerializeField] private List<Stat> statList = new();

        /*
         * PRESET
         */
        #region GetStatValue List

        public float Power => GetStatValue(StatType.Power);
        public float Health => GetStatValue(StatType.Health);
        public float CriticalChance => GetStatValue(StatType.CriticalChance);
        public float CriticalDamage => GetStatValue(StatType.CriticalDamage);
        public float Haste => GetStatValue(StatType.Haste);
        public float Armor => GetStatValue(StatType.Armor);
        public float MoveSpeed => GetStatValue(StatType.MoveSpeed);
        public float MaxHp => GetStatValue(StatType.MaxHp);
        public float MaxResource => GetStatValue(StatType.MaxResource);
        public float MinWeaponValue => GetStatValue(StatType.MinDamage);
        public float MaxWeaponValue => GetStatValue(StatType.MaxDamage);
        public float ExtraPower => GetStatValue(StatType.ExtraPower);
        public float ExtraCriticalChance => GetStatValue(StatType.ExtraCriticalChance);
        public float ExtraCriticalDamage => GetStatValue(StatType.ExtraCriticalDamage);

        #endregion

        public void Add(Stat stat) => statList.AddUniquely(stat);
        public void Add(StatType statType, string statKey, float value)
        {
            statList.Add(new Stat(statType, statKey, value));
            statList.Sort((x, y) => x.StatType.CompareTo(y.StatType));
        }

        public void Remove(Stat stat) => statList.RemoveSafely(stat);
        public void Remove(StatType statType, float value)
        {
            foreach (var stat in statList)
            {
                if (stat.StatType != statType || Math.Abs(stat.Value - value) > 0.0001f) continue;
                
                statList.Remove(stat);
                statList.Sort((x, y) => x.StatType.CompareTo(y.StatType));
                return;
            }
        }

        public void Clear() => statList.Clear();
        public void Iterate(Action<Stat> action) => statList.ForEach(action.Invoke);
        public float GetStatValue(StatType statType)
        {
            var stat = statList.TryGetElement(element => element.StatType == statType);

            return stat is not null ? stat.Value : 0f;
        }

        public static Spec operator +(Spec a, Spec b)
        {
            var result = new Spec();
            
            a.Iterate(result.Add);
            b.Iterate(result.Add);

            return result;
        }

        // TODO. 가급적 StatTable에서 Add하는 걸로 진행해보고, 가능하다면 아래 함수들은 삭제.
        public void Register(StatTable table) => table.Add(this);
        public void Unregister(StatTable table) => table.Remove(this);
    }
}
