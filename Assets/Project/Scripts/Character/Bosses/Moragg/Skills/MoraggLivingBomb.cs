using Common.Completion;
using Common.Skills;
using UnityEngine;

namespace Monsters.Moragg.Skills
{
    public class MoraggLivingBomb : SkillComponent
    {
        [SerializeField] private DeBuffCompletion livingBomb;


        protected override void Initialize()
        {
            livingBomb.Initialize(Cb);
            
            OnActivated.Register("PlayAnimation", PlayAnimation);
            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            
            OnHit.Register("MoraggLivingBomb", OnAttack);

            OnCompleted.Register("StartCooling", StartCooling);
            OnCompleted.Register("EndCallback", End);
            
            OnEnded.Register("ReleaseHit", UnregisterHitEvent);
        }

        protected override void Dispose()
        {
            // TODO. Unregister Sequence Events;
        }
        
        private void OnAttack()
        {
            if (MainTarget is null) return;
            
            livingBomb.DeBuff(MainTarget);
        }
    }
}
