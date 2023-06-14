using Common.Skills;

namespace Character.Villains.Moragg.Skills
{
    public class MoraggCommonAttack : SkillComponent
    {
        public override void Execution()
        {
            if (MainTarget is null) return;
            
            Execute(MainTarget);
        }

        protected override void Initialize()
        {
            OnCompleted.Register("EndCallback", End);
        }
    }
}
