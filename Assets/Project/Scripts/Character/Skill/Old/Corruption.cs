namespace Character.Combat.Skill
{
    public class Corruption : SkillObject
    {
        private void OnCorruptionActivated()
        {
            if (StatusEffectModule && TargetModule)
            {
                TargetModule.TakeStatusEffect(StatusEffectModule);
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            OnActivated.Register(InstanceID, OnCorruptionActivated);
        }
    }
}
