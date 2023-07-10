using Common.Skills;

namespace Character.Villains.Commons.Skills
{
    public class VillainCommonAttack : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            Builder.Add(Section.Execute, "CommonExecution", HitVillainCommonAttack);
        }


        private void HitVillainCommonAttack()
        {
            Invoker.Hit(MainTarget);
        }
    }
}
