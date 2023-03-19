using Common.Skills;

namespace Character.Bosses.Moragg.Skills
{
    public class MoraggLivingBomb : SkillComponent
    {
        public override void MainAttack()
        {
            if (MainTarget is null) return;

            Completion(MainTarget);
        }
        

        protected override void Initialize()
        {
            OnCompleted.Register("MoraggLivingBomb", MainAttack);
            OnCompleted.Register("EndCallback", End);
        }
    }
}
