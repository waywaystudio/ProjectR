using Common.Skills;

namespace Character.Venturers.Hunter.Skills
{
    public class ShotAttack : SkillComponent
    {
        public override void Execution()
        {
            ExecutionTable.Execute(null);
        }
        
        protected override void Initialize()
        {
            OnCompleted.Register("EndCallback", End);
        }
    }
}
