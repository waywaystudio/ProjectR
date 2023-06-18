using Common.Skills;

namespace Character.Venturers.Rogue.Skills
{
    public class PowerBeat : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            // AddAnimationEvent();
            
            SequenceBuilder.Add(SectionType.Execute, "CommonExecution", () =>
            {
                if (!TryGetTakersInSphere(this, out var takerList)) return;

                takerList.ForEach(executor.Execute);
            });
        }
    }
}
