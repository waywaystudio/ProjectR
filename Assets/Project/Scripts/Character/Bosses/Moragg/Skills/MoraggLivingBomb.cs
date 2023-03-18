using Common.Completion;
using Common.Skills;
using UnityEngine;

namespace Monsters.Moragg.Skills
{
    public class MoraggLivingBomb : SkillSequence
    {
        [SerializeField] private DeBuffCompletion livingBomb;

        public override void OnAttack()
        {
            if (MainTarget is null) return;
            
            livingBomb.Completion(MainTarget);
        }
        

        protected override void Initialize()
        {
            livingBomb.Initialize(Cb);
            
            OnActivated.Register("PlayAnimation", PlayAnimation);
            
            OnCompleted.Register("MoraggLivingBomb", OnAttack);
            OnCompleted.Register("StartCooling", StartCooling);
            OnCompleted.Register("EndCallback", End);
        }
    }
}
