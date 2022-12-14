using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class ValueTable
    {
        [ShowInInspector] protected Dictionary<string, double> SumTable = new();
        [ShowInInspector] protected Dictionary<string, float> MultiplyTable = new();
        
        /// <summary>
        /// Return PlusTable.Sum() + MultiplyTable.Multiply.
        /// Project R Calculate Sum Value First.
        /// </summary>
        /// <returns>Calculated Value</returns>
        [ShowInInspector] public double Result => Sum() * MultiPly();
        public float ResultToFloat => (float)Sum() * MultiPly();

        /// <summary>
        /// Register SumType Value.
        /// if Table has already key, compare absolute Value and select bigger.
        /// </summary>
        /// <param>custom key (ex.Equipment)
        ///     <name>key</name>
        /// </param>
        /// <param>double type value
        ///     <name>value</name>
        /// </param>
        public void RegisterSumType(string key, double value, bool compare = true)
        {
            if (compare)
            {
                if (SumTable.ContainsKey(key))
                {
                    if (Math.Abs(SumTable[key]) < Math.Abs(value))
                    {
                        SumTable[key] = value;
                    }
                }

                SumTable.TryAdd(key, value);
                return;
            }
            
            if (SumTable.ContainsKey(key))
            {
                SumTable[key] = value;
                return;
            }

            SumTable.TryAdd(key, value);
        }

        public void RegisterSumTypeOverwrite(string key, double value)
        {
            if (SumTable.ContainsKey(key))
            {
                SumTable[key] = value;
            }
            
            SumTable.TryAdd(key, value);
        }

        public void UnregisterSumType(string key) => SumTable.TryRemove(key);
        
        /// <summary>
        /// Register MultiplyType Value.
        /// if Table has already key, compare absolute Value and select bigger.
        /// </summary>
        /// </summary>
        /// <param>custom key (ex.Equipment)
        ///     <name>key</name>
        /// </param>
        /// <param>double type value
        ///     <name>value</name>
        /// </param>
        public void RegisterMultiplyType(string key, float value, bool compare = true)
        {
            if (compare)
            {
                if (MultiplyTable.ContainsKey(key))
                {
                    if (Math.Abs(MultiplyTable[key]) < Math.Abs(value))
                    {
                        MultiplyTable[key] = value;
                    }
                }

                MultiplyTable.TryAdd(key, value);
                return;
            }
            
            if (MultiplyTable.ContainsKey(key))
            {
                MultiplyTable[key] = value;
                return;
            }

            MultiplyTable.TryAdd(key, value);
        }
        
        public void RegisterMultiplyTypeOverwrite(string key, float value)
        {
            if (MultiplyTable.ContainsKey(key))
            {
                MultiplyTable[key] = value;
            }
            
            MultiplyTable.TryAdd(key, value);
        }
        
        public void UnregisterMultiplyType(string key) => MultiplyTable.TryRemove(key);
        
        
        private double Sum()
        {
            var result = 0d;
            
            SumTable.ForEach(x => result += x.Value);

            return result;
        }
        private float MultiPly()
        {
            var result = 1.0f;
            
            MultiplyTable.ForEach(x =>
            {
                if (x.Value <= 0.0f)
                {
                    Debug.LogWarning($"{x.Value} multiplied from : {x.Key}");
                }
                
                result *= x.Value;
            });

            return result;
        }
    }
}
