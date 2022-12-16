using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core
{
    public abstract class BaseValueTable<T> where T : IComparable
    {
        [ShowInInspector] protected Dictionary<string, Func<T>> SumTable = new();
        [ShowInInspector] protected Dictionary<string, Func<float>> MultiTable = new();
        [ShowInInspector]
        public abstract T Result { get; }
        protected float Multiply
        {
            get
            {
                var result = 1.0f;
            
                MultiTable.ForEach(x =>
                {
                    if (x.Value.Invoke() == 0.0f) Debug.LogWarning($"0 multiplied from : {x.Key}");
                    result *= x.Value.Invoke();
                });

                return result;
            }
        }
        
        /// <summary>
        /// Register SumType Value.
        /// if Table has already key, compare absolute Value and select bigger.
        /// highly recommend on static value. (ex. field)
        /// </summary>
        public void RegisterSumType(string key, T value, bool overwrite = false) => RegisterType(SumTable, key, () => value, overwrite);
        
        
        /// <summary>
        /// Register SumType Value.
        /// if Table has already key, compare absolute Value and select bigger.
        /// highly recommend on dynamic value. (ex. property, function)
        /// </summary>
        public void RegisterSumType(string key, Func<T> value, bool overwrite = false) => RegisterType(SumTable, key, value, overwrite);
        
        /// <summary>
        /// Register MultiplyType Value.
        /// if Table has already key, compare absolute Value and select bigger.
        /// highly recommend on static value.
        /// </summary>
        public void RegisterMultiType(string key, float value, bool overwrite = false) => RegisterType(MultiTable, key, () => value, overwrite);
        
        /// <summary>
        /// Register MultiplyType Value.
        /// if Table has already key, compare absolute Value and select bigger.
        /// highly recommend on dynamic value.
        /// </summary>
        public void RegisterMultiType(string key, Func<float> value, bool overwrite = false) => RegisterType(MultiTable, key, value, overwrite);
        public void UnregisterSumType(string key) => SumTable.TryRemove(key);
        public void UnregisterMultiType(string key) => MultiTable.TryRemove(key);

        protected void RegisterType<TValue>(IDictionary<string, Func<TValue>> table, string key, Func<TValue> value, bool overwrite = false) where TValue : IComparable
        {
            if (table.ContainsKey(key))
            {
                if (overwrite || table[key].Invoke().CompareTo(value) == -1)
                    table[key] = value;
            }
            else
                table.Add(key, value);
        }
        
        public static bool operator >(BaseValueTable<T> tableA, BaseValueTable<T> tableB) => tableA.Result.CompareTo(tableB.Result) == -1;
        public static bool operator <(BaseValueTable<T> tableA, BaseValueTable<T> tableB) => tableA.Result.CompareTo(tableB.Result) == 1;
    }

    [Serializable]
    public class DoubleTable : BaseValueTable<double>
    {
        /// <summary>
        /// Return PlusTable.Sum + MultiplyTable.Multiply.
        /// Project R Calculate Sum Value First.
        /// </summary>
        /// <returns>Calculated Value</returns>
        public override double Result => Sum * Multiply;
        private double Sum
        {
            get
            {
                var value = 0d;
                SumTable.ForEach(x => value += x.Value.Invoke());
                return value;
            }
        }
    }
    
    [Serializable]
    public class FloatTable : BaseValueTable<float>
    {
        /// <summary>
        /// Return PlusTable.Sum + MultiplyTable.Multiply.
        /// Project R Calculate Sum Value First.
        /// </summary>
        /// <returns>Calculated Value</returns>
        public override float Result => Sum * Multiply;
        private float Sum
        {
            get
            {
                var value = 0f;
                SumTable.ForEach(x => value += x.Value.Invoke());
                return value;
            }
        }
    }
}
