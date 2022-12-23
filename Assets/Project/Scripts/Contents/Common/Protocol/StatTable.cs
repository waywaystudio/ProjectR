#if UNITY_EDITOR
using System.Reflection;
using Sirenix.OdinInspector.Editor;
#endif

using System;
using System.Collections.Generic;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class StatTable : Dictionary<StatCode, FloatTable>
    {
        // Preset
        public float Power => Get(StatCode.Power);
        public float Critical => Get(StatCode.Critical);
        public float Haste => Get(StatCode.Haste);
        public float Hit => Get(StatCode.Hit);
        public float MaxHp => Get(StatCode.MaxHp);
        public float MoveSpeed => Get(StatCode.MoveSpeed);
        public float Armor => Get(StatCode.Armor);
        public float Evade => Get(StatCode.Evade);
        public float Resist => Get(StatCode.Resist);

        public void Register(StatCode statCode, int partKey, float value, bool overwrite = false)
        {
            if (ContainsKey(statCode)) this[statCode].Register(partKey, value, overwrite);
            else Add(statCode, new FloatTable{{ partKey, value }});
        }
        public void Register(StatCode statCode, int partKey, Func<float> function, bool overwrite = false)
        {
            if (ContainsKey(statCode)) this[statCode].Register(partKey, function.Invoke(), overwrite);
            else Add(statCode, new FloatTable{{ partKey, function.Invoke() }});
        }
        public void Register(StatCode statCode, FloatTable table, bool overwrite = false)
        {
            if (ContainsKey(statCode) && overwrite) this[statCode] = table;
            else TryAdd(statCode, table);
        }
        
        public void Unregister(StatCode statCode, int partKey)
        {
            if (ContainsKey(statCode)) this[statCode].Unregister(partKey);
        }
        public void Unregister(StatCode statCode) => this.TryRemove(statCode);
        
        public void UnionWith(StatTable target)
        {
            target.ForEach(x =>
            {
                TryAdd(x.Key, x.Value);
            });
        }

        public static StatTable operator +(StatTable left, StatTable right)
        {
            var mainTable = new StatTable();

            if (left.IsNullOrEmpty())
            {
                if (right.IsNullOrEmpty()) return mainTable;

                mainTable = right.MemberwiseClone() as StatTable;
                return mainTable;
            }

            mainTable = left.MemberwiseClone() as StatTable;
                
            if (right.IsNullOrEmpty())
                return mainTable;

            mainTable.ForEach(floatTable =>
            {
                if (right.ContainsKey(floatTable.Key))
                {
                    right[floatTable.Key].ForEach(x => floatTable.Value.Register(x.Key, x.Value, true));
                }
            });

            right.ForEach(x => mainTable?.TryAdd(x.Key, x.Value));

            return mainTable;
        }

        public float Get(string statName)
        {
            var statCode = statName.ToEnum<StatCode>();

            return Get(statCode);
        }
        public float Get(StatCode statCode)
        {
            var codeIndex = (int)statCode;
            
            switch (codeIndex)
            {
                case < 101:
                {
                    var add = GetValue((StatCode)(codeIndex + 100));
                    var multi = GetValue((StatCode)(codeIndex + 1000));
                    return add * (1f + multi);
                }
                case < 1001:
                {
                    var add = GetValue((StatCode)(codeIndex + 100));
                    return add;
                }
                case < 10001:
                {
                    var multi = GetValue((StatCode)(codeIndex + 1000));
                    return multi;
                }
                default:
                {
                    Debug.LogError("statCode Missing return -1.0f");
                    return -1.0f;
                }
            }
        }
        private float GetValue(StatCode statCode) => TryGetValue(statCode, out var floatTable) ? floatTable.Result : 0f;
    }
    
    /*
     * Annotation
     * Register, Unregister 할 때 값을 계산해서 해당 스탯을 갱신하면 더 좋을 듯?
     */

    [Serializable]
    public class FloatTable : Dictionary<int, float>
    {
        public float Result
        {
            get
            {
                var result = 0f;
                this.ForEach(x => result += x.Value);
                return result;
            }
        }

        public void Register(int key, float function, bool overwrite)
        {
            if (ContainsKey(key) && overwrite) this[key] = function;
            else 
                TryAdd(key, function);
        }

        public void Unregister(int key) => this.TryRemove(key);

        /*
         * Annotation
         * Debugging 용 Dictionary<string, float> 을 만들어도 좋을 듯 하다.
         */
    }

#if UNITY_EDITOR
    public class CastingEntityDrawer : OdinAttributeProcessor<FloatTable>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new DictionaryDrawerSettings
            {
               DisplayMode = DictionaryDisplayOptions.OneLine,
               IsReadOnly = true,
            });
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "CastingTime")
            {
                // attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
#endif
}
