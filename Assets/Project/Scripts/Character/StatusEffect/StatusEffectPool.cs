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
        
        public void OnStatusEffectGet(StatusEffectComponent statusEffect)
        {
            statusEffect.gameObject.SetActive(true);
            statusEffect.transform.SetParent(parent);
            statusEffect.Initialize(provider);
        }

        public void OnStatusEffectRelease(StatusEffectComponent statusEffect)
        {
            statusEffect.gameObject.SetActive(false);
            statusEffect.transform.SetParent(Origin);
        }
        
        public void OnStatusEffectDestroy(StatusEffectComponent statusEffect)
        {
            Destroy(statusEffect.gameObject);
        }
    }
}
