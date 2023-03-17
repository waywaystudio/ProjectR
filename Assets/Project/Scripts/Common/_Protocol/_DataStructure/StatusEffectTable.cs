using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class StatusEffectTable : Dictionary<(ICombatProvider, DataIndex), IStatusEffect>
    {
        private event System.Action OnEffectChanged;

        public void AddListener(System.Action action) => OnEffectChanged += action;
        public void RemoveListener(System.Action action) => OnEffectChanged -= action;

        public void Register(IStatusEffect statusEffect)
        {
            var key = (statusEffect.Provider, statusEffect.ActionCode);
            
            if (!ContainsKey(key))
            {
                Add(key, statusEffect);
                OnEffectChanged?.Invoke();
            }
            else
            {
                this[key] = statusEffect;
            }
        }

        public void Unregister(IStatusEffect statusEffect)
        {
            this.TryRemove((statusEffect.Provider, statusEffect.ActionCode));
            
            OnEffectChanged?.Invoke();
        }
    }
}