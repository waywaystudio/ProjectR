using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace Core
{
    public abstract class ValueTable<T> : Dictionary<string, Func<T>> where T : IComparable
    {
        public abstract T Result { get; }

        /// <summary>
        /// Register NumberType Value in Table.
        /// highly recommend on static value. (ex. field)
        /// </summary>
        public void Register(string key, T value, bool overwrite = false) => Register(key, () => value, overwrite);
        
        /// <summary>
        /// Register NumberType Value in Table.
        /// highly recommend on dynamic value. (ex. property, function)
        /// </summary>
        public void Register(string key, Func<T> value, bool overwrite = false)
        {
            if (ContainsKey(key))
            {
                if (overwrite ||
                    this[key].Invoke().CompareTo(value.Invoke()) == -1 ||
                    this[key].Invoke().CompareTo(value.Invoke()) == 0)
                    this[key] = value;
            }
            else
            {
                Add(key, value);
            }
        }

        public void Unregister(string key) => ContainsKey(key).OnTrue(() => Remove(key));
        public void UnregisterAll() => Clear();
    }

    public abstract class SumTable<T> : ValueTable<T> where T : IComparable
    {
        public override T Result => Sum;
        protected abstract T Sum { get; }
    }
    public abstract class MultiTable<T> : ValueTable<T> where T : IComparable
    {
        public override T Result => Multiply;
        protected abstract T Multiply { get; }
    }

    public class DoubleSumTable : SumTable<double>
    {
        protected override double Sum
        {
            get
            {
                var result = 0d;
                this.ForEach(x => result += x.Value.Invoke());
                return result;
            }
        }
    }
    public class FloatSumTable : SumTable<float>
    {
        protected override float Sum
        {
            get
            {
                var result = 0f;
                this.ForEach(x => result += x.Value.Invoke());
                return result;
            }
        }
    }
    public class LongSumTable : SumTable<long>
    {
        protected override long Sum
        {
            get
            {
                var result = 0L;
                this.ForEach(x => result += x.Value.Invoke());
                return result;
            }
        }
    }
    public class IntSumTable : SumTable<int>
    {
        protected override int Sum
        {
            get
            {
                var result = 0;
                this.ForEach(x => result += x.Value.Invoke());
                return result;
            }
        }
    }

    public class DoubleMultiTable : MultiTable<double>
    {
        protected override double Multiply
        {
            get
            {
                var result = 1d;
                this.ForEach(x => result *= x.Value.Invoke());
                return result;
            }
        }
    }
    public class FloatMultiTable : MultiTable<float>
    {
        protected override float Multiply
        {
            get
            {
                var result = 1f;
                this.ForEach(x => result *= x.Value.Invoke());
                return result;
            }
        }
    }
    public class LongMultiTable : MultiTable<long>
    {
        protected override long Multiply
        {
            get
            {
                var result = 1L;
                this.ForEach(x => result *= x.Value.Invoke());
                return result;
            }
        }
    }
    public class IntMultiTable : MultiTable<int>
    {
        protected override int Multiply
        {
            get
            {
                var result = 1;
                this.ForEach(x => result *= x.Value.Invoke());
                return result;
            }
        }
    }

    public class BoolTable : ValueTable<bool>
    {
        public override bool Result => this.All(x => x.Value.Invoke());
        public bool Any => this.Any(x => x.Value.Invoke());
    }
}