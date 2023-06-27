using System;
using Common.StatusEffects;

namespace Common
{
    public class StatusEffectTable : Table<DataIndex, StatusEffect>
    {
        private event Action OnEffectChanged;
        
        public void AddListener(Action action) => OnEffectChanged += action;
        public void RemoveListener(Action action) => OnEffectChanged -= action;
        
        public override void Add(DataIndex key, StatusEffect statusEffect)
        {
            base.Add(key, statusEffect);
            
            OnEffectChanged?.Invoke();
        }
        
        public override bool Remove(DataIndex effectIndex)
        {
            var result = base.Remove(effectIndex);
            
            OnEffectChanged?.Invoke();

            return result;
        }

        public bool TryGetEffect<T>(DataIndex key, out T effect) where T : StatusEffect
        {
            if (Map.TryGetValue(key, out var value))
            {
                var result = value as T;

                effect = result;
                return true;
            }

            effect = null;
            return false;
        }
    }
}