using Common.StatusEffect;
using UnityEngine;
using UnityEngine.Pool;

namespace Common.Completion
{
    public class DeBuffCompletion : CollidingCompletion, IEditable
    {
        [SerializeField] private GameObject prefabReference;
        [SerializeField] private int poolCapacity;

        private IObjectPool<StatusEffectSequence> pool;
        
        
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);
            
            pool = new ObjectPool<StatusEffectSequence>(OnCreatePool,
                null,
                null,
                (statusEffect) => statusEffect.Dispose(),
                true,
                poolCapacity);
        }

        public override void Completion(ICombatTaker taker)
        {
            if (!taker.DynamicStatEntry.Alive.Value) return;
            
            var targetTable = taker.DynamicStatEntry.DeBuffTable;
            var key         = (provider: Provider, statusCode: actionCode);

            if (targetTable.ContainsKey(key))
            {
                targetTable[key].Overriding();
            }
            
            else
            {
                var effect = Get();
                
                effect.transform.SetParent(taker.StatusEffectHierarchy, false);
                effect.Execution(taker);

                effect.Provider.OnDeBuffProvided.Invoke(effect);
                taker.OnDeBuffTaken.Invoke(effect);
            }
        }

     
        private StatusEffectSequence OnCreatePool()
        {
            if (!prefabReference.IsNullOrEmpty() && Instantiate(prefabReference).TryGetComponent(out StatusEffectSequence component))
            {
                component.Initialize(Provider);
                component.OnEnded.Register("ReturnToPool", () => component.transform.SetParent(transform, false));
                
                return component;
            }
            
            Debug.LogError($"Not Exist {nameof(StatusEffectSequence)} in prefab:{prefabReference.name}. return null");
            return null;
        }
        
        private StatusEffectSequence Get() => pool.Get();


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            prefabReference.TryGetComponent(out IStatusEffect statusEffectInfo);
            actionCode = statusEffectInfo.ActionCode;
        }
#endif
    }
}
