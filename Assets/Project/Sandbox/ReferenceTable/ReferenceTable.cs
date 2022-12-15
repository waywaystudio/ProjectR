using System;
using System.Collections.Generic;
using Core;
using Sirenix.OdinInspector;

namespace ReferenceTable
{
    public class ValueTableExample
    {
        private FloatRefTable floatTable = new();
        private DoubleRefTable doubleTable = new();

        private void Example()
        {
            var floatValue = floatTable.Result;
            var doubleValue = doubleTable.Result;
        }
    }

    public abstract class ReferenceTable<T> where T : IComparable
    {
        [ShowInInspector] protected Dictionary<string, Func<T>> SumTable = new();
        [ShowInInspector] protected Dictionary<string, Func<float>> MultiTable = new();

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

        public void RegisterSumType(string key, T value, bool overwrite = false) => RegisterType(SumTable, key, () => value, overwrite);
        public void RegisterSumType(string key, Func<T> value, bool overwrite = false) => RegisterType(SumTable, key, value, overwrite);
        public void RegisterMultiType(string key, float value, bool overwrite = false) => RegisterType(MultiTable, key, () => value, overwrite);
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
        
        public static bool operator >(ReferenceTable<T> tableA, ReferenceTable<T> tableB) => tableA.Result.CompareTo(tableB.Result) == -1;
        public static bool operator <(ReferenceTable<T> tableA, ReferenceTable<T> tableB) => tableA.Result.CompareTo(tableB.Result) == 1;
    }
    
    public class FloatRefTable : ReferenceTable<float>
    {
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

        public static float operator +(FloatRefTable tableA, FloatRefTable tableB) => tableA.Result + tableB.Result;
        public static float operator -(FloatRefTable tableA, FloatRefTable tableB) => tableA.Result - tableB.Result;
        public static float operator *(FloatRefTable tableA, FloatRefTable tableB) => tableA.Result * tableB.Result;
        public static float operator /(FloatRefTable tableA, FloatRefTable tableB) => tableA.Result / tableB.Result;
    }
    
    public class DoubleRefTable : ReferenceTable<double>
    {
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
        
        public static double operator +(DoubleRefTable tableA, DoubleRefTable tableB) => tableA.Result + tableB.Result;
        public static double operator -(DoubleRefTable tableA, DoubleRefTable tableB) => tableA.Result - tableB.Result;
        public static double operator *(DoubleRefTable tableA, DoubleRefTable tableB) => tableA.Result * tableB.Result;
        public static double operator /(DoubleRefTable tableA, DoubleRefTable tableB) => tableA.Result / tableB.Result;
    }
}
