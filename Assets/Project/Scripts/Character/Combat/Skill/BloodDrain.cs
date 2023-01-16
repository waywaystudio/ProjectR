namespace Character.Combat.Skill
{
    public class BloodDrain : SkillObject
    {
        private void OnBloodDrainActive()
        {
            if (TargetModule && StatusEffectModule)
            {
                TargetModule.TakeStatusEffect(StatusEffectModule);
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            OnActivated.Register(InstanceID, OnBloodDrainActive);
        }
    }
}
