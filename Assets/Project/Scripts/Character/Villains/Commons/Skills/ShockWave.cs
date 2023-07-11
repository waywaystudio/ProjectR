using Common.Characters;
using Common.Skills;

namespace Character.Villains.Commons.Skills
{
    public class ShockWave : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            Builder
                .Add(Section.Execute, "CommonExecution", HitShockWave);
        }


        private void HitShockWave()
        {
            detector.GetTakers()?.ForEach(Invoker.Hit);
        }
    }
}
