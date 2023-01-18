using Core;
using UnityEngine;
using UnityEngine.Pool;

namespace Character.Combat
{
    using StatusEffects;
    
    public class StatusEffectModule : CombatModule
    {
        // TODO. 이후에는, IDCode 통해서 풀링하고, GameObject Field를 삭제하자.
        [SerializeField] private GameObject statusEffectPrefab;
        [SerializeField] private DataIndex statusEffectID;
        [SerializeField] private int maxPool = 2;
        
        private ICombatTaker taker;
        private IObjectPool<StatusEffectObject> pool;
        
        public ICombatProvider Provider => CombatObject.Provider;

        public void Effectuate(ICombatTaker taker)
        {
            this.taker = taker;

            pool.Get();
        }
        
        
        protected StatusEffectObject CreateStatusEffect()
        {
            var statusEffect = Instantiate(statusEffectPrefab).GetComponent<StatusEffectObject>();
            
            statusEffect.SetPool(pool);

            return statusEffect;
        }

        protected void OnStatusEffectGet(StatusEffectObject statusEffect)
        {
            statusEffect.gameObject.SetActive(true);
            statusEffect.Initialize(Provider, taker);
        }

        protected static void OnStatusEffectRelease(StatusEffectObject projectile)
        {
            projectile.gameObject.SetActive(false);
        }

        protected static void OnStatusEffectDestroy(StatusEffectObject projectile)
        {
            Destroy(projectile.gameObject);
        }

        protected override void Awake()
        {
            base.Awake();

            pool = new ObjectPool<StatusEffectObject>(
                CreateStatusEffect,
                OnStatusEffectGet,
                OnStatusEffectRelease,
                OnStatusEffectDestroy,
                maxSize: maxPool);
        }


#if UNITY_EDITOR
        public void SetUpValue(DataIndex effectCode)
        {
            Flag             = ModuleType.StatusEffect;
            statusEffectID = effectCode;
        }
#endif
    }
}