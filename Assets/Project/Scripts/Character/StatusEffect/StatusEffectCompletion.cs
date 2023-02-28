using Core;

namespace Character.StatusEffect
{
    // => ProjectileCompletion
    public class StatusEffectCompletion : Pool<StatusEffectComponent>
    {
        private ICombatProvider provider;
        private IStatusEffect effectInfo;


        public void DeBuff(ICombatTaker taker)
        {
            var targetTable = taker.DynamicStatEntry.DeBuffTable;
            var key         = (Provider: provider, effectInfo.ActionCode);

            if (targetTable.ContainsKey(key))
            {
                targetTable[key].OnOverride();
            }
            else
            {
                var effect = Get();
                
                effect.Active(provider, taker);
                effect.transform.SetParent(taker.StatusEffectHierarchy);

                taker.TakeDeBuff(effect);
            }
        }

        public void Buff(ICombatTaker taker)
        {
            var targetTable = taker.DynamicStatEntry.BuffTable;

            var key = (Provider: provider, effectInfo.ActionCode);

            if (targetTable.ContainsKey(key))
            {
                targetTable[key].OnOverride();
            }
            else
            {
                var effect = Get();
                
                effect.Active(provider, taker);
                effect.transform.SetParent(taker.StatusEffectHierarchy);

                taker.TakeDeBuff(effect);
            }
        }
        
        public void Effect(ICombatTaker taker)
        {
            var targetTable = effectInfo.Type == StatusEffectType.Buff
                ? taker.DynamicStatEntry.BuffTable
                : taker.DynamicStatEntry.DeBuffTable;

            var key = (Provider: provider, effectInfo.ActionCode);

            if (targetTable.ContainsKey(key))
            {
                targetTable[key].OnOverride();
            }
            else
            {
                var effect = Get();
                
                effect.Active(provider, taker);
                effect.transform.SetParent(taker.StatusEffectHierarchy);

                taker.TakeDeBuff(effect);
            }
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
            Destroy(statusEffect.gameObject);
        }

        protected override void Awake()
        {
            base.Awake();
            
            provider = GetComponentInParent<ICombatProvider>();
            prefab.TryGetComponent(out effectInfo);
        }
    }
}
