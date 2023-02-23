using Character.StatusEffect;
using UnityEngine;

namespace Character.Skill.Moragg
{
    public class MoraggLivingBomb : GeneralAttack
    {
        [SerializeField] private StatusEffectPool statusEffectPool;


        protected override void OnAttack()
        {
            if (MainTarget is null) return;
            
            var effectInfo = statusEffectPool.Effect;
            var table = MainTarget.DynamicStatEntry.DeBuffTable;
            
            if (table.ContainsKey((Provider, effectInfo.ActionCode)))
            {
                table[(Provider, effectInfo.ActionCode)].OnOverride();
            }
            else
            {
                MainTarget.TakeDeBuff(statusEffectPool.Get());
            }
        }

        protected override void OnEnable()
        {
            statusEffectPool.Initialize(Provider);
            
            base.OnEnable();
            
            OnCompleted.Register("StartCooling", StartCooling);
        }
    }
}
