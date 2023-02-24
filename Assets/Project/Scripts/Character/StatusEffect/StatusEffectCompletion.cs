using Core;

namespace Character.StatusEffect
{
    // => ProjectileCompletion
    public class StatusEffectCompletion : Pool<StatusEffectComponent>
    {
        private ICombatProvider Provider { get; set; }
        private IStatusEffect effectInfo;


        public void Effect(ICombatTaker taker)
        {
            var targetTable = effectInfo.Type == StatusEffectType.Buff
                ? taker.DynamicStatEntry.BuffTable
                : taker.DynamicStatEntry.DeBuffTable;

            var key = (Provider, effectInfo.ActionCode);

            if (targetTable.ContainsKey(key))
            {
                targetTable[key].OnOverride();
            }
            else
            {
                var effect = Get();
                
                effect.Active(Provider, taker);
                effect.transform.SetParent(taker.StatusEffectHierarchy);

                taker.TakeStatusEffect(effect);
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
            
            Provider = GetComponentInParent<ICombatProvider>();
            prefab.TryGetComponent(out effectInfo);
        }
    }
}
