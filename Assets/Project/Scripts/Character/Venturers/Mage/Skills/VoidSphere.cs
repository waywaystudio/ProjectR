using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Mage.Skills
{
    public class VoidSphere : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            SequenceBuilder.Add(SectionType.Execute, "ShotAttackExecution", () => executor.Execute(null))
                           .Add(SectionType.Execute, "PlayCastCompleteAnimation", PlayCastCompleteAnimation);

        }
        
        
        private void PlayCastCompleteAnimation()
        {
            Cb.Animating.PlayOnce("CastHoldFire", 1f + Haste, SkillInvoker.Complete);
        }
    }
}
