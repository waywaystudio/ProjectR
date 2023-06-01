using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common
{
    public class EthosTable
    {
        #region Preset
        [ShowInInspector] public int Vacillation => GetEthosValue(EthosType.Vacillation);
        [ShowInInspector] public int Fortitude => GetEthosValue(EthosType.Fortitude);
        [ShowInInspector] public int Obstinacy => GetEthosValue(EthosType.Obstinacy);
        [ShowInInspector] public int Cowardice => GetEthosValue(EthosType.Cowardice);
        [ShowInInspector] public int Valor => GetEthosValue(EthosType.Valor);
        [ShowInInspector] public int Recklessness => GetEthosValue(EthosType.Recklessness);
        [ShowInInspector] public int Rash => GetEthosValue(EthosType.Rash);
        [ShowInInspector] public int Cunning => GetEthosValue(EthosType.Cunning);
        [ShowInInspector] public int Obsession => GetEthosValue(EthosType.Obsession);
        [ShowInInspector] public int Conventionality => GetEthosValue(EthosType.Conventionality);
        [ShowInInspector] public int Creativity => GetEthosValue(EthosType.Creativity);
        [ShowInInspector] public int Reverie => GetEthosValue(EthosType.Reverie);
        [ShowInInspector] public int Ignorance => GetEthosValue(EthosType.Ignorance);
        [ShowInInspector] public int Wisdom => GetEthosValue(EthosType.Wisdom);
        [ShowInInspector] public int Arrogance => GetEthosValue(EthosType.Arrogance);
        [ShowInInspector] public int Apathy => GetEthosValue(EthosType.Apathy);
        [ShowInInspector] public int Benevolence => GetEthosValue(EthosType.Benevolence);
        [ShowInInspector] public int Voracity => GetEthosValue(EthosType.Voracity);
        #endregion
        
        private Dictionary<EthosType, EthosSet> Table { get; } = new();
        private List<EthosTable> ReferenceTable { get; } = new();

        public void RegisterTable(EthosTable   anotherTable) => ReferenceTable.AddUniquely(anotherTable);
        public void UnregisterTable(EthosTable anotherTable) => ReferenceTable.RemoveSafely(anotherTable);
        
        public void Add(EthosSpec ethosSpec) => ethosSpec?.IterateOverStats(Add);
        public void Add(EthosEntity ethos)
        {
            if (ethos == null) return;
            if (Table.TryGetValue(ethos.EthosType, out var value))
            {
                value.Add(ethos);
            }
            else
            {
                var newTable = new EthosSet();
                newTable.Add(ethos);
                
                Table.Add(ethos.EthosType, newTable);
            }
        }
        
        public void Remove(EthosSpec ethosSpec) => ethosSpec?.IterateOverStats(Remove);
        public void Remove(EthosEntity ethos)
        {
            if (ethos == null) return;
            if (!Table.ContainsKey(ethos.EthosType)) return;
            
            Table[ethos.EthosType].Remove(ethos);
        }

        public void Clear() => Table.Clear();

        public int GetEthosValue(EthosType ethosType)
        {
            if (ethosType.IsVice())
            {
                var result = !Table.ContainsKey(ethosType) ? 0 : Table[ethosType].Value;
            
                ReferenceTable.ForEach(otherTable =>
                {
                    result += otherTable.GetEthosValue(ethosType);
                });
        
                return result;
            }

            var deficiency = GetEthosValue(ethosType.PrevExceptNone());
            var excess     = GetEthosValue(ethosType.NextExceptNone());

            return Mathf.Min(deficiency, excess);
        }
        
        public class EthosSet
        {
            private readonly Dictionary<string, EthosEntity> table = new();
            public int Value { get; private set; }

            public void Add(EthosEntity ethos)
            {
                table[ethos.EthosKey] = ethos;

                ethos.OnValueChanged -= Calculate;
                ethos.OnValueChanged += Calculate;
                Calculate(ethos);
            }
            
            public void Remove(EthosEntity ethos)
            {
                var key = ethos.EthosKey;
                
                if (!table.ContainsKey(key)) return;
                
                ethos.OnValueChanged -= Calculate;
                table.TryRemove(key);
                Calculate(ethos);
            }
            

            private void Calculate(EthosEntity ethos)
            {
                Value = 0;
                table.Values.ForEach(ethosEntity => Value += ethosEntity.Value);
            }
        }
    }
}
