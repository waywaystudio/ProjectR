namespace Character.Combat.Skill
{
    public class Smash : SkillObject
    {
        protected override void OnAssigned()
        {
            OnActivated.Register(InstanceID, OnSmashActivated);
            // OnCompleted.Register(InstanceID, OnSmashCompleted);
        }
        
        private void OnSmashCompleted()
        {
            if (TargetModule && ProjectorModule)
            {
                TargetModule.TakeProjector(ProjectorModule);
            }
        }
        
        private void OnSmashActivated()
        {
            if (TargetModule && ProjectorModule)
            {
                TargetModule.TakeProjector(ProjectorModule);
            }
        }
    }
}
