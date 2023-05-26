using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class EthosSpec
    {
        #region Preset
        public int Vacillation => GetEthosValue(EthosType.Vacillation);
        public int Fortitude => GetEthosValue(EthosType.Fortitude);
        public int Obstinacy => GetEthosValue(EthosType.Obstinacy);
        public int Cowardice => GetEthosValue(EthosType.Cowardice);
        public int Valor => GetEthosValue(EthosType.Valor);
        public int Recklessness => GetEthosValue(EthosType.Recklessness);
        public int Rash => GetEthosValue(EthosType.Rash);
        public int Cunning => GetEthosValue(EthosType.Cunning);
        public int Obsession => GetEthosValue(EthosType.Obsession);
        public int Conventionality => GetEthosValue(EthosType.Conventionality);
        public int Creativity => GetEthosValue(EthosType.Creativity);
        public int Reverie => GetEthosValue(EthosType.Reverie);
        public int Ignorance => GetEthosValue(EthosType.Ignorance);
        public int Wisdom => GetEthosValue(EthosType.Wisdom);
        public int Arrogance => GetEthosValue(EthosType.Arrogance);
        public int Apathy => GetEthosValue(EthosType.Apathy);
        public int Benevolence => GetEthosValue(EthosType.Benevolence);
        public int Voracity => GetEthosValue(EthosType.Voracity);
        #endregion
        
        [SerializeField] private List<EthosEntity> ethosList = new();

        public EthosSpec() { }
        public EthosSpec(EthosType type = EthosType.None, string key = "", int stack = 1)
        {
            ethosList.Add(new EthosEntity(type, key, stack));
        }

        public void Add(EthosEntity ethos) => ethosList.AddUniquely(ethos);
        public void Add(EthosType type, string key, int value)
        {
            ethosList.Add(new EthosEntity(type, key, value));
            ethosList.Sort((x, y) => x.EthosType.CompareTo(y.EthosType));
        }

        public void Remove(EthosEntity ethos) => ethosList.RemoveSafely(ethos);
        
        public void Clear() => ethosList.Clear();
        
        
        
        public int GetEthosValue(EthosType ethosType)
        {
            var ethos = ethosList.TryGetElement(element => element.EthosType == ethosType);

            return ethos is not null ? ethos.Value : 0;
        }
        
        public void IterateOverStats(Action<EthosEntity> action) => ethosList.ForEach(action.Invoke);
        public void IterateOverStats(Action<EthosEntity, int> action) => ethosList.ForEach(action.Invoke);
        public static EthosSpec operator +(EthosSpec a, EthosSpec b)
        {
            var result = new EthosSpec();
            
            a.IterateOverStats(result.Add);
            b.IterateOverStats(result.Add);

            return result;
        }
        
        public static EthosSpec operator +(EthosSpec a, int adder)
        {
            var result = new EthosSpec();
            
            a.IterateOverStats(result.Add);
            
            result.IterateOverStats(ethos => ethos.Value += adder);

            return result;
        }
    }
}
