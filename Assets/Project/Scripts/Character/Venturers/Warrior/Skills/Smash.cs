using Common.Characters;
using Common.Skills;

namespace Character.Venturers.Warrior.Skills
{
    public class Smash : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            cost.PayCondition.Add("HasTarget", detector.HasTarget);

            Builder
                .Add(Section.Execute, "SmashAttack",SmashAttack);
        }


        private void SmashAttack()
        {
            detector.GetTakers()?.ForEach(Invoker.Hit);
        }
    }
}
