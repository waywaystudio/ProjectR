namespace Common.Characters.Behaviours.CrowdControlEffect
{
    public class DeadBehaviour : ActionBehaviour
    {
        public override CharacterActionMask BehaviourMask => CharacterActionMask.Dead;
        public override CharacterActionMask IgnorableMask => CharacterActionMask.DeadIgnoreMask;
        
        protected bool IsAble => Conditions.IsAllTrue 
                                 && CanOverrideToCurrent;

        public void Dead()
        {
            if (!IsAble) return;
            
            RegisterBehaviour(Cb);
            
            OnActivated.Invoke();
            Cb.Pathfinding.Quit();
            Cb.Animating.Dead();
        }
    }
}
