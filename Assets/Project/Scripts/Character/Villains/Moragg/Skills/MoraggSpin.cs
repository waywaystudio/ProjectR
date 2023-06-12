using Common;
using Common.Skills;
using UnityEngine;

namespace Character.Villains.Moragg.Skills
{
    public class MoraggSpin : SkillComponent, IOldProjectorSequence
    {
        public Vector2 SizeVector => new(range, 60);
        
        public override void Execution()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;
            
            takerList.ForEach(ExecutionTable.Execute);
        }
        

        protected override void PlayAnimation()
        {
            Cb.Animating.PlayLoop(animationKey);
        }

        protected override void Initialize()
        {
            OnCompleted.Register("MoraggSpinAttack", Execution);
            OnCompleted.Register("PlayEndCastingAnimation", PlayEndCastingAnimation);
        }

        private void PlayEndCastingAnimation()
        {
            Cb.Animating.PlayOnce("attack", 0f, End);
        }

        
    }
}
