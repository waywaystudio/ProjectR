using Common.StatusEffect;
using UnityEngine;

namespace Common.Execution
{
    public class DeBuffExecutor : ExecuteComponent, IEditable
    {
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] private Pool<StatusEffectComponent> pool;

        public override void Execution(ICombatTaker taker, float instantMultiplier = 1f)
        {
            if (!taker.DynamicStatEntry.Alive.Value) return;

            var targetTable = taker.DynamicStatEntry.DeBuffTable;
            var tableKey    = new StatusEffectTable.StatusEffectKey(Executor.Provider, actionCode);

            if (targetTable.TryGetValue(tableKey, out var value))
            {
                value.Overriding();
            }
            else
            {
                var effect = pool.Get();
                
                effect.transform.SetParent(taker.StatusEffectHierarchy, false);
                effect.Activate(taker);

                effect.Provider.OnDeBuffProvided.Invoke(effect);
                taker.OnDeBuffTaken.Invoke(effect);
            }
        }
        
        
        private void CreateStatusEffect(StatusEffectComponent statusEffect)
        {
            statusEffect.Initialize(Executor.Provider);
            statusEffect.OnEnded.Register("ReturnToPool", () =>
            {
                statusEffect.transform.SetParent(transform, false);
                pool.Release(statusEffect);
            });
        }

        private void OnEnable()
        {
            pool.Initialize(CreateStatusEffect, transform);
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
            pool.Prefab.TryGetComponent(out IStatusEffect statusEffectInfo);
            actionCode = statusEffectInfo.ActionCode;
        }
#endif
    }
}
