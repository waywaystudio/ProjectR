using Common.Skills;

namespace Character.Venturers.Knight.Skills
{
    public class SwordAttack : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            cost.PayCondition.Add("HasTarget", ExecuteSwordAttack);

            SequenceBuilder.Add(SectionType.Execute, "CommonExecution", 
                                () => detector.GetTakers()?.ForEach(executor.Execute));
        }


        private bool ExecuteSwordAttack()
        {
            var takers = detector.GetTakers();

            return !takers.IsNullOrEmpty();
        }
    }
}

