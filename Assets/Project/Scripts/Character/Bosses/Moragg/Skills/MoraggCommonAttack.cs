using Common.Completion;
using Common.Skills;
using UnityEngine;

namespace Character.Bosses.Moragg.Skills
{
    public class MoraggCommonAttack : SkillComponent
    {
        [SerializeField] protected DamageCompletion damage;
        [SerializeField] protected DeBuffCompletion armorCrash;
        

        protected override void Initialize()
        {
            damage.Initialize(Cb, ActionCode);
            armorCrash.Initialize(Cb);

            OnActivated.Register("PlayAnimation", PlayAnimation);
            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            
            OnHit.Register("MoraggCommonAttack", OnAttack);
            OnCompleted.Register("EndCallback", End);
            OnEnded.Register("ReleaseHit", UnregisterHitEvent);
        }

        protected override void Dispose()
        {
            // TODO. Unregister Sequence Events;
        }
        
        private void OnAttack()
        {
            damage.Damage(MainTarget);
            armorCrash.DeBuff(MainTarget);
        }
    }
}
