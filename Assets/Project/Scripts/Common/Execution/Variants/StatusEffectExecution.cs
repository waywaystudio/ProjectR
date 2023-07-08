using Common.StatusEffects;
using UnityEngine;

namespace Common.Execution.Variants
{
    public class StatusEffectExecution : HitExecution, IEditable
    {
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] private Pool<StatusEffect> pool;

        public override void Hit(ICombatTaker taker)
        {
            if (taker == null || !taker.Alive.Value) return;

            var targetTable = taker.StatusEffectTable;

            if (targetTable.TryGetValue(actionCode, out var value))
            {
                value.Invoker.Override();
            }
            else
            {
                var effect = pool.Get();

                effect.transform.SetParent(taker.StatusEffectHierarchy, false);
                effect.Activate(taker);
            }
        }
        
        
        private void CreateStatusEffect(StatusEffect statusEffect)
        {
            statusEffect.Initialize(Sender.Provider);

            var builder = new SequenceBuilder(statusEffect.Sequence);

            builder.Add(Section.End, "ReturnToPool", () => ReturnToPool(statusEffect));
        }

        private void ReturnToPool(StatusEffect statusEffect)
        {
            statusEffect.transform.SetParent(transform, false);
            pool.Release(statusEffect);
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
            // pool.Prefab.TryGetComponent(out StatusEffect statusEffectInfo);
        }
#endif
    }
}
