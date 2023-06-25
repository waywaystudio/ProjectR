using Common.Skills;

namespace Character.Venturers.Rogue.Skills
{
    public class DoubleStab : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            cost.PayCondition.Add("HasTarget", HasTarget);

            SequenceBuilder.Add(SectionType.Execute, "CommonExecution", 
                                () => detector.GetTakers()?.ForEach(executor.Execute));
        }


        private bool HasTarget()
        {
            var takers = detector.GetTakers();

            return !takers.IsNullOrEmpty() 
                   && takers[0].DynamicStatEntry.Alive.Value;
        }
    }
}
