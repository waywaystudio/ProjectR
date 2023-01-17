namespace Character.Combat.Skill
{
    public class Fury : SkillObject
    {
        private void OnFuryActivated()
        {
            if (TargetModule && StatusEffectModule)
            {
                TargetModule.TakeStatusEffect(StatusEffectModule);
            }
        }
        
        protected override void OnAssigned()
        {
            OnActivated.Register(InstanceID, OnFuryActivated);
        }
    }
}
