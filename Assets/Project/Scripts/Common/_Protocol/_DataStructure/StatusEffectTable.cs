using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common
{
    public class StatusEffectTable : Dictionary<StatusEffectTable.StatusEffectKey, IStatusEffect>
    {
        private event System.Action OnEffectChanged;

        public void AddListener(System.Action action) => OnEffectChanged += action;
        public void RemoveListener(System.Action action) => OnEffectChanged -= action;

        public void Register(IStatusEffect statusEffect)
        {
            var key = new StatusEffectKey(statusEffect.Provider, statusEffect.ActionCode);
            // (statusEffect.Provider, statusEffect.ActionCode);
            
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
            this.TryRemove(new StatusEffectKey(statusEffect.Provider, statusEffect.ActionCode));
                //((statusEffect.Provider, statusEffect.ActionCode));
            
            OnEffectChanged?.Invoke();
        }
        
        [Serializable]
        public class StatusEffectKey
        {
            [ShowInInspector]
            public ICombatProvider Provider { get; }
            public DataIndex ActionCode { get; }

            public StatusEffectKey(ICombatProvider provider, DataIndex actionCode)
            {
                Provider   = provider;
                ActionCode = actionCode;
            }

            // Override GetHashCode and Equals for proper dictionary key handling
            public override int GetHashCode()
            {
                return (Provider, ActionCode).GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj is StatusEffectKey other)
                {
                    return other.Provider == Provider && other.ActionCode.Equals(ActionCode);
                }
                return false;
            }
        }
    }
}