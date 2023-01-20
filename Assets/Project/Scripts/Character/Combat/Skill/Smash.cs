namespace Character.Combat.Skill
{
    public class Smash : SkillObject
    {
        protected override void OnAssigned()
        {
            OnCompleted.Register(InstanceID, OnSmashCompleted);
        }
        
        private void OnSmashCompleted()
        {
            if (TargetModule && ProjectorModule)
            {
                TargetModule.TakeProjector(ProjectorModule);
            }
        }
    }
}
