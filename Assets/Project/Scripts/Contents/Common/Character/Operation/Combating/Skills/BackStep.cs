namespace Common.Character.Operation.Combating.Skills
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
