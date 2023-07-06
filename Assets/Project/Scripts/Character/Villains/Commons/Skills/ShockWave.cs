using Common.Skills;

namespace Character.Villains.Commons.Skills
{
    public class ShockWave : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            Builder.Add(Section.Execute, "PlayOnceCompleteAnimation",() => Cb.Animating.PlayOnce("attack", 1f + Haste, Invoker.Complete))
                           .Add(Section.Execute, "CommonExecution", () => detector.GetTakers()?.ForEach(executor.ToTaker));
        }
    }
}
