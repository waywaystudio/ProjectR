using Common.Skills;

namespace Character.Villains.Moragg.Skills
{
    public class MoraggCommonAttack : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            SequenceBuilder.Add(SectionType.Execute, "CommonExecution", 
                                () => detector.GetTakers()?.ForEach(executor.Execute));
        }
    }
}
