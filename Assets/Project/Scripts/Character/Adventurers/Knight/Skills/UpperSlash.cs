using Common.Skills;

namespace Character.Adventurers.Knight.Skills
{
    public class UpperSlash : SkillComponent
    {
        public override void Execution()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;
        
            takerList.ForEach(Executor.Execute);
        }

        protected override void Initialize()
        {
            OnCompleted.Register("EndCallback", End);
        }
    }
}
