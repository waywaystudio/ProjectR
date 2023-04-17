using Common.StatusEffect;
using UnityEngine;
using UnityEngine.Pool;

namespace Common.Execution
{
    public class DeBuffExecutor : ExecuteComponent, IEditable
    {
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] private GameObject prefabReference;
        [SerializeField] private int poolCapacity;

        private IObjectPool<StatusEffectComponent> pool;


        public override void Execution(ICombatTaker taker, float instantMultiplier = 1f)
        {
            if (!taker.DynamicStatEntry.Alive.Value) return;
            
            var targetTable = taker.DynamicStatEntry.DeBuffTable;
            var tableKey         = (Executor.Provider, actionCode);

            if (targetTable.TryGetValue(tableKey, out var value))
            {
                value.Overriding();
            }
            
            else
            {
                var effect = Get();
                
                effect.transform.SetParent(taker.StatusEffectHierarchy, false);
                effect.Activate(taker);

                effect.Provider.OnDeBuffProvided.Invoke(effect);
                taker.OnDeBuffTaken.Invoke(effect);
            }
        }
        
        private StatusEffectComponent OnCreatePool()
        {
            if (!prefabReference.IsNullOrEmpty() && Instantiate(prefabReference).TryGetComponent(out StatusEffectComponent component))
            {
                component.Initialize(Executor.Provider);
                component.OnEnded.Register("ReturnToPool", () =>
                {
                    component.transform.SetParent(transform, false);
                    Release(component);
                });
                
                return component;
            }
            
            Debug.LogError($"Not Exist {nameof(StatusEffectComponent)} in prefab:{prefabReference.name}. return null");
            return null;
        }
        
        private StatusEffectComponent Get() => pool.Get();
        private void Release(StatusEffectComponent element) => pool.Release(element);

        private void OnEnable()
        {
            pool = new ObjectPool<StatusEffectComponent>(OnCreatePool,
                null,
                null,
                (component) => component.Dispose(),
                true,
                poolCapacity);
            
            Executor?.ExecutionTable.Add(this);
        }

        private void OnDisable()
        {
            pool.Clear();
            
            Executor?.ExecutionTable.Remove(this);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            if (TryGetComponent(out Skills.SkillComponent skill))
            {
                var statusEffectCode = (DataIndex)Database.SkillSheetData(skill.ActionCode).StatusEffect;
                
                Database.StatusEffectPrefabData.GetObject(statusEffectCode, out prefabReference);
            }
            else if (!TryGetComponent(out prefabReference))
            {
                Debug.LogError("At least SkillComponent or StatusEffectComponent In Same Inspector");
            }

            prefabReference.TryGetComponent(out IStatusEffect statusEffectInfo);
            actionCode = statusEffectInfo.ActionCode;
        }
#endif
    }
}
