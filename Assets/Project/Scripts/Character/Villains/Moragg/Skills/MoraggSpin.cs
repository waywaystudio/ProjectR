using Common.Skills;
using UnityEngine;

namespace Character.Villains.Moragg.Skills
{
    public class MoraggSpin : SkillComponent, IProjectorSections
    {
        public new Vector2 SizeVector => new(range, 60);
        
        public override void Execution() => ExecuteAction.Invoke();

        protected override void PlayAnimation()
        {
            Cb.Animating.PlayLoop(animationKey);
        }

        protected override void AddSkillSequencer()
        {
            
            
            ExecuteAction.Add("CommonExecution", () =>
            {
                if (TryGetTakersInSphere(this, out var takerList))
                {
                    takerList.ForEach(executor.Execute);
                }
        
                Cb.Animating.PlayOnce("attack", 0f, SkillSequencer.Complete);
            });
        }
    }
}
