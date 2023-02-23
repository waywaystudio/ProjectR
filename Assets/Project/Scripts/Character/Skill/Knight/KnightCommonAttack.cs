using Character.StatusEffect;
using UnityEngine;

namespace Character.Skill.Knight
{
    public class KnightCommonAttack : GeneralAttack
    {
        [SerializeField] private StatusEffectPool statusEffectPool;

        protected override void OnAttack()
        {
            if (!colliding.TryGetTakersInSphere(transform.position, range, angle, targetLayer, out var takerList)) return;
            
            takerList.ForEach(taker =>
            {
                var combatEntity = taker.TakeDamage(this);
                Debug.Log($"{Provider.Name} provider {ActionCode.ToString()}.{combatEntity.Value} to {taker.Name}");

                var effectInfo = statusEffectPool.Effect;
                var table = taker.DynamicStatEntry.DeBuffTable;

                if (table.ContainsKey((Provider, effectInfo.ActionCode)))
                {
                    table[(Provider, effectInfo.ActionCode)].OnOverride();
                }
                else
                {
                    taker.TakeDeBuff(statusEffectPool.Get());
                }
            });
        }
        
        protected override void OnEnable()
        {
            statusEffectPool.Initialize(Provider);

            base.OnEnable();
        }
    }
}
