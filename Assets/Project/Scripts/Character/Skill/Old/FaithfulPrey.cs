namespace Character.Combat.Skill
{
    public class FaithfulPrey : SkillObject
    {
        private void OnFaithfulPreyCompleted()
        {
            if (HealModule && TargetModule)
            {
                TargetModule.TakeHeal(HealModule);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            
            OnCompleted.Register(InstanceID, OnFaithfulPreyCompleted);
        }
    }
}
