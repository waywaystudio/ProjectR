using Common;
using Common.Skills;

namespace Character.Adventurers.Hunter.Skills
{
    public class Trap : SkillComponent
    {
        public override ICombatTaker MainTarget => Cb.Searching.GetSelf();
        
        public override void Execution()
        {
            ExecutionTable.Execute(MainTarget);
        }
        

        protected override void Initialize()
        {
            OnActivated.Register("Jump", Jump);
            OnActivated.Register("Execution", Execution);
            OnCompleted.Register("EndCallback", End);
        }

        private void Jump()
        {
            var direction = Cb.transform.forward * -1f;
            
            Cb.Pathfinding.Jump(direction, 10f, 2, 0.5f);
        }
    }
}
