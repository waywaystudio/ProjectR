using Common.Skills;

namespace Character.Bosses.Moragg.Skills
{
    public class MoraggCommonAttack : SkillComponent
    {
        public override void MainAttack()
        {
            if (MainTarget is null) return;

            Completion(MainTarget);
        }
        

        protected override void Initialize()
        {
            OnCompleted.Register("EndCallback", End);
        }
    }
}
