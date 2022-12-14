using System;
using System.Collections.Generic;
// ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator

namespace Core
{
    public class StatValueTable : Dictionary<DataIndex, StatValue>
    {
        public float Result;
        public Action OnResultChanged { get; set; }

        public void Register(DataIndex key, StatValue value, bool overwrite)
        {
            if (!ContainsKey(key)) Add(key, value);
            else
            {
                if (!overwrite) return;
                this[key] = value;
            }
            
            value.RegisterUniquely(ReCalculation);
            ReCalculation();
        }

        public void Register(DataIndex key, StatValue value)
        {
            if (!ContainsKey(key)) Add(key, value);
            else
            {
                if (Abs(value.Value) > Abs(this[key].Value))
                {
                    this[key].Unregister((int)key);
                    this[key] = value;
                }
                else return;
            }
            
            value.RegisterUniquely(ReCalculation);
            ReCalculation();
        }
        
        public void Unregister(DataIndex key)
        {
            this.TryRemove(key);
            ReCalculation();
        }

        public void ReCalculation()
        {
            Result = 0f;
            foreach (var item in this) Result += this[item.Key].Value;
            OnResultChanged?.Invoke();
        }

        private static float Abs(float value) => value >= 0 ? value : value * -1.0f;
    }
}
