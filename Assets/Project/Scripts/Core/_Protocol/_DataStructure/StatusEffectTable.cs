using System.Collections.Generic;

namespace Core
{
    public class StatusEffectTable : Dictionary<(ICombatProvider, DataIndex), IStatusEffect>
    {
        public void Register(IStatusEffect statusEffect, out bool isOverride)
        {
            var key = (statusEffect.Provider, statusEffect.ActionCode);
            
            if (!ContainsKey(key))
            {
                Add(key, statusEffect);
                isOverride = false;
            }
            else
            {
                this[key] = statusEffect;
                isOverride = true;
            }
        }
        
        public void Register(IStatusEffect statusEffect)
        {
            var key = (statusEffect.Provider, statusEffect.ActionCode);
            
            if (!ContainsKey(key))
            {
                Add(key, statusEffect);
            }
            else
            {
                this[key] = statusEffect;
            }
        }

        public void Unregister(IStatusEffect statusEffect) 
            => this.TryRemove((statusEffect.Provider, statusEffect.ActionCode));
    }
}
