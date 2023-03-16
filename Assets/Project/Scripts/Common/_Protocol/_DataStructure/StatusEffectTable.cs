using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class StatusEffectTable : Dictionary<(ICombatProvider, DataIndex), IStatusEffect>
    {
        public ActionTable OnEffectChanged { get; } = new();

        public void Register(IStatusEffect statusEffect)
        {
            var key = (statusEffect.Provider, statusEffect.ActionCode);
            
            if (!ContainsKey(key))
            {
                Add(key, statusEffect);
                OnEffectChanged.Invoke();
            }
            else
            {
                Debug.LogWarning("Duplicate Key inserted in Table Level. " +
                                 "Check Duplicate logic. (this message should not be called)");
                this[key] = statusEffect;
            }
        }

        public void Unregister(IStatusEffect statusEffect)
        {
            this.TryRemove((statusEffect.Provider, statusEffect.ActionCode));
            OnEffectChanged.Invoke();
        }
    }
}