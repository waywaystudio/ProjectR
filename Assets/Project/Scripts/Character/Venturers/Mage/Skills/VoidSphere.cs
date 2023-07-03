using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Mage.Skills
{
    public class VoidSphere : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            Builder.Add(SectionType.Execute, "Fire", Fire)
                           .Add(SectionType.Execute, "PlayCastCompleteAnimation", PlayCastCompleteAnimation);

        }
        
        
        private void Fire()
        {
            var forwardPosition = Provider.Position + Provider.Forward;

            executor.ToPosition(forwardPosition);
        }
        
        private void PlayCastCompleteAnimation()
        {
            Cb.Animating.PlayOnce("CastHoldFire", 1f + Haste, Invoker.Complete);
        }
    }
}
