using Common.Character.Skills.Core;

namespace Common.Character.Skills
{
    public class BackStep : BaseSkill
    {
        public override void StartSkill()
        {
            base.StartSkill();
            
            // target.TakeDamage();

            CompleteSkill();
        }
    }
}
