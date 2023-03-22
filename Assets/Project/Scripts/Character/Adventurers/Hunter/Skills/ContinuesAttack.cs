using Common.Skills;

namespace Character.Adventurers.Hunter.Skills
{
    public class ContinuesAttack : SkillComponent
    {
        public override void Execution()
        {
            ExecutionTable.Execute(null);
        }
        

        protected override void Initialize()
        {
            OnCompleted.Register("EndCallback", End);
        }

        protected override void PlayAnimation()
        {
            Cb.Animating.PlayLoop(animationKey);
        }
    }
}
