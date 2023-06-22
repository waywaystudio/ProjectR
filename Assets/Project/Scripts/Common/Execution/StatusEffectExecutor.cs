using Common.StatusEffects;
using UnityEngine;

namespace Common.Execution
{
    public class StatusEffectExecutor : ExecuteComponent, IEditable
    {
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] private Pool<StatusEffect> pool;

        public override void Execution(ICombatTaker taker)
        {
            if (taker == null || !taker.DynamicStatEntry.Alive.Value) return;

            var targetTable = taker.DynamicStatEntry.StatusEffectTable;
            var tableKey    = new StatusEffectKey(Origin.Provider, actionCode);

            if (targetTable.TryGetValue(tableKey, out var value))
            {
                value.Overriding();
            }
            else
            {
                var effect = pool.Get();

                effect.transform.SetParent(taker.StatusEffectHierarchy, false);
                effect.Activate(taker);
                
                taker.TakeStatusEffect(effect);
            }
        }
        
        
        private void CreateStatusEffect(StatusEffect statusEffect)
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
            pool.Prefab.TryGetComponent(out StatusEffect statusEffectInfo);
            actionCode = statusEffectInfo.DataIndex;
        }
#endif
    }
}
