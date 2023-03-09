using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core
{
    [ShowInInspector]
    public class StatTable : Dictionary<StatCode, StatValueTable>
    {
        public StatTable() : this(1) { }
        public StatTable(int capacity) : base(capacity) { }

        public float Power { get; set; }
        public float Critical { get; set; }
        public float Haste { get; set; }
        public float MaxHp { get; set; }
        public float MaxResource { get; set; }
        public float MoveSpeed { get; set; }
        public float Armor { get; set; }

        public void Register(DataIndex key, StatValue statEntity, bool overwrite)
        {
            if (!ContainsKey(statEntity.StatCode))
            {
                /* 순서 엄청 중요하다. 
                 1. assign callback, 
                 2. table add, 
                 3. valueTable Register */
                var statValueTable = new StatValueTable { OnResultChanged = () => Recalculation(statEntity.StatCode) };

                Add(statEntity.StatCode, statValueTable);
                statValueTable.Register(key, statEntity, overwrite);
            }
            else
                this[statEntity.StatCode].Register(key, statEntity, overwrite);
        }
        
        public void Register(DataIndex key, StatValue statEntity)
        {
            if (!ContainsKey(statEntity.StatCode))
            {
                var statValueTable = new StatValueTable { OnResultChanged = () => Recalculation(statEntity.StatCode) };

                Add(statEntity.StatCode, statValueTable);
                statValueTable.Register(key, statEntity);
            }
            else
                this[statEntity.StatCode].Register(key, statEntity);
        }
        
        public void Register(StatCode statCode, DataIndex dataIndex, float value, bool overwrite)
        {
            var statValue = new StatValue(value) { StatCode = statCode };
            
            if (!ContainsKey(statCode))
            {
                var statValueTable = new StatValueTable { OnResultChanged = () => Recalculation(statCode) };

                Add(statCode, statValueTable);
                statValueTable.Register(dataIndex, statValue, overwrite);
            }
            else
                this[statCode].Register(dataIndex, statValue, overwrite);
        }
        
        public void Unregister(DataIndex key, StatValue statValue) => Unregister(key, statValue.StatCode);
        public void Unregister(DataIndex key, StatCode statCode)
        {
            if (ContainsKey(statCode)) this[statCode].Unregister(key);
        }

        public void UnionWith(StatTable target, bool overwrite = true)
        {
            target.ForEach(statEntry =>
            {
                statEntry.Value.ForEach(statEntityTable =>
                {
                    Register(statEntityTable.Key, statEntityTable.Value, overwrite);
                });
            });
        }
        

        private void Recalculation(StatCode statCode)
        {
            var result = this[statCode].Result;
            
            switch (statCode)
            {
                case StatCode.Power : Power             = result; break;
                case StatCode.Critical : Critical       = result; break;
                case StatCode.Haste : Haste             = result; break;
                case StatCode.MaxHp: MaxHp              = result; break;
                case StatCode.MaxResource : MaxResource = result; break;
                case StatCode.MoveSpeed : MoveSpeed     = result; break;
                case StatCode.Armor : Armor             = result; break;
                case StatCode.None:
                {
                    Debug.LogError($"statCode Missing. Input:{StatCode.None}");
                    return;
                }
                default:
                {
                    Debug.LogError($"statCode Missing. Input:{statCode}");
                    return;
                }
            }
        }
    }
}