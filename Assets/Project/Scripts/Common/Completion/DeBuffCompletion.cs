using Common.StatusEffect;
using UnityEngine;

namespace Common.Completion
{
    public class DeBuffCompletion : Pool<StatusEffectComponent>
    {
        private ICombatProvider provider;
        private DataIndex statusCode;

        public void Initialize(ICombatProvider provider)
        {
            this.provider = provider;
            
            prefab.TryGetComponent(out IStatusEffect statusEffectInfo);
            statusCode = statusEffectInfo.ActionCode;
        }

        public void DeBuff(ICombatTaker taker)
        {
            if (!taker.DynamicStatEntry.Alive.Value) return;
            
            var targetTable = taker.DynamicStatEntry.DeBuffTable;
            var key         = (provider, statusCode);

            if (targetTable.ContainsKey(key))
            {
                targetTable[key].OnOverride();
            }
            
            else
            {
                var effect = Get();
                
                effect.transform.SetParent(taker.StatusEffectHierarchy);
                effect.Execution(taker);

                effect.Provider.OnDeBuffProvided.Invoke(effect);
                taker.OnDeBuffTaken.Invoke(effect);
            }
        }

        
        protected override StatusEffectComponent OnCreatePool()
        {
            if (!prefab.IsNullOrEmpty() && Instantiate(prefab).TryGetComponent(out StatusEffectComponent component))
            {
                component.Initialized(provider);

                component.Pool = this;
                return component;
            }
            
            Debug.LogError($"Not Exist {nameof(StatusEffectComponent)} in prefab:{prefab.name}. return null");
            return null;
        }

        protected override void OnGetPool(StatusEffectComponent statusEffect)
        {
            statusEffect.gameObject.SetActive(true);
        }

        protected override void OnReleasePool(StatusEffectComponent statusEffect)
        {
            statusEffect.gameObject.SetActive(false);
            statusEffect.transform.SetParent(Origin, false);
        }
        
        protected override void OnDestroyPool(StatusEffectComponent statusEffect)
        {
            statusEffect.Disposed();
        }
    }
}
