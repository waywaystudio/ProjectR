namespace Common.Characters.Behaviours.Movement
{
    public class StopBehaviour : ActionBehaviour
    {
        public override CharacterActionMask BehaviourMask => CharacterActionMask.Stop;
        public override CharacterActionMask IgnorableMask => CharacterActionMask.None;
        
        public void Active()
        {
            OnActivated.Invoke();
            
            Cb.Pathfinding.Stop();
            Cb.Animating.Idle();
            Cb.CurrentBehaviour = this;
        }
    }
}
