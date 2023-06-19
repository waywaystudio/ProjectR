using Common.StatusEffect;
using UnityEngine;

namespace Common.Execution
{
    public class DeBuffExecutor : ExecuteComponent, IEditable
    {
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] private Pool<StatusEffectComponent> pool;

        public override void Execution(ICombatTaker taker)
        {
            if (taker == null || !taker.DynamicStatEntry.Alive.Value) return;

            var targetTable = taker.DynamicStatEntry.DeBuffTable;
            var tableKey    = new StatusEffectTable.StatusEffectKey(Origin.Provider, actionCode);

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
            statusEffect.Initialize(Origin.Provider);

            var builder = new SequenceBuilder(statusEffect.Sequencer);

            builder.Add(SectionType.End, "ReturnTransform", () => statusEffect.transform.SetParent(transform, false))
                   .Add(SectionType.End, "ReleasePool", () => pool.Release(statusEffect));
        }

        private void OnEnable()
        {
            pool.Initialize(CreateStatusEffect, transform);
        }

        private void OnDisable()
        {
            pool.Clear();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            pool.Prefab.TryGetComponent(out IStatusEffect statusEffectInfo);
            actionCode = statusEffectInfo.DataIndex;
        }
#endif
    }
}
