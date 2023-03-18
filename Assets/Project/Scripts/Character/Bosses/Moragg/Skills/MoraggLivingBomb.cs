using Common.Skills;

namespace Monsters.Moragg.Skills
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
