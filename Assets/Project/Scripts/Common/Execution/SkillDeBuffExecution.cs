using Common.Skills;
using Common.StatusEffect;
using UnityEngine;
using UnityEngine.Pool;

namespace Common.Execution
{
    public class SkillDeBuffExecution : ExecuteComponent, IEditable
    {
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] private GameObject prefabReference;
        [SerializeField] private int poolCapacity;
        
        private ICombatProvider provider;
        private IObjectPool<StatusEffectComponent> pool;


        public override void Execution(ICombatTaker taker, float instantMultiplier)
        {
            if (!taker.DynamicStatEntry.Alive.Value) return;
            
            var targetTable = taker.DynamicStatEntry.DeBuffTable;
            var tableKey         = (provider, actionCode);

            if (targetTable.ContainsKey(tableKey))
            {
                targetTable[tableKey].Overriding();
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
        
        private StatusEffectComponent OnCreatePool()
        {
            if (!prefabReference.IsNullOrEmpty() && Instantiate(prefabReference).TryGetComponent(out StatusEffectComponent component))
            {
                component.Initialize(provider);
                component.OnEnded.Register("ReturnToPool", () => component.transform.SetParent(transform, false));
                
                return component;
            }
            
            Debug.LogError($"Not Exist {nameof(StatusEffectComponent)} in prefab:{prefabReference.name}. return null");
            return null;
        }
        
        private StatusEffectComponent Get() => pool.Get();

        private void Awake()
        {
            if (!TryGetComponent(out SkillComponent skill))
            {
                Debug.LogError("Require SkillComponent In Same Inspector");
                return;
            }
            
            provider = skill.Cb;
            pool = new ObjectPool<StatusEffectComponent>(OnCreatePool,
                null,
                null,
                (statusEffect) => statusEffect.Dispose(),
                true,
                poolCapacity);
            
            skill.Executor.Add(this);
            // skill.OnCompletion.Register("DeBuff", Execution);
        }
        
        
#if UNITY_EDITOR
        public void EditorSetUp()
        {
            if (!TryGetComponent(out SkillComponent skill))
            {
                Debug.LogError("Require SkillComponent In Same Inspector");
                return;
            }

            var statusEffectCode = (DataIndex)Database.SkillSheetData(skill.ActionCode).StatusEffect;

            Database.StatusEffectMaster.GetObject(statusEffectCode, out prefabReference);
            
            prefabReference.TryGetComponent(out IStatusEffect statusEffectInfo);
            actionCode = statusEffectInfo.ActionCode;
        }
#endif
    }
}
