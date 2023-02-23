using Core;

namespace Character.StatusEffect
{
    public class StatusEffectPool : Pool<StatusEffectComponent>
    {
        private ICombatProvider provider;
        private IStatusEffect effect;

        public IStatusEffect Effect => effect;

        public void Initialize(ICombatProvider provider)
        {
            this.provider = provider;

            prefab.TryGetComponent(out effect);
        }
        
        protected override void OnGetPool(StatusEffectComponent statusEffect)
        {
            statusEffect.gameObject.SetActive(true);
            statusEffect.Initialize(provider);
        }

        protected override void OnReleasePool(StatusEffectComponent statusEffect)
        {
            statusEffect.gameObject.SetActive(false);
            // statusEffect.transform.SetParent(Origin, false);
        }
        
        protected override void OnDestroyPool(StatusEffectComponent statusEffect)
        {
            Destroy(statusEffect.gameObject);
        }
    }
}
