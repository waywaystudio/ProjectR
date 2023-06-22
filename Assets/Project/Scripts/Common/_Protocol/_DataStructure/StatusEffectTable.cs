using System;
using System.Collections.Generic;
using Common.StatusEffects;

namespace Common
{
    public class StatusEffectTable
    {
        private readonly Dictionary<StatusEffectKey, StatusEffect> table = new();
        private event Action OnEffectChanged;

        public void AddListener(Action action) => OnEffectChanged += action;
        public void RemoveListener(Action action) => OnEffectChanged -= action;

        public void Add(StatusEffect statusEffect)
        {
            if (!table.ContainsKey(statusEffect.Key))
            {
                table.Add(statusEffect.Key, statusEffect);
                OnEffectChanged?.Invoke();
            }
            else
            {
                table[statusEffect.Key] = statusEffect;
            }
        }

        public void Remove(StatusEffect effect)
        {
            if (table.ContainsKey(effect.Key))
            {
                table.Remove(effect.Key);
            }

            OnEffectChanged?.Invoke();
        }

        public void Iterator(Action<StatusEffect> action)
        {
            foreach (var statusEffect in table.Values) action?.Invoke(statusEffect);
        }

        public bool TryGetValue(StatusEffectKey key, out StatusEffect value)
        {
            return table.TryGetValue(key, out value);
        }
    }
}