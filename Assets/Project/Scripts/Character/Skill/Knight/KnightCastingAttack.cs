using Character.StatusEffect;
using UnityEngine;

namespace Character.Skill.Knight
{
    public class KnightCastingAttack : CastingAttack
    {
        [SerializeField] private StatusEffectPool statusEffectPool;


        protected override void OnCastingAttack()
        {
            if (!colliding.TryGetTakersInSphere(transform.position, range, angle, targetLayer, out var takerList)) return;
            
            takerList.ForEach(taker =>
            {
                taker.TakeDamage(this);

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
