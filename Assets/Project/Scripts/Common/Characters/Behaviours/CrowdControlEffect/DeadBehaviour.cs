namespace Common.Characters.Behaviours.CrowdControlEffect
{
    public class DeadBehaviour : ActionBehaviour
    {
        public override CharacterActionMask BehaviourMask => CharacterActionMask.Dead;
        public override CharacterActionMask IgnorableMask => CharacterActionMask.None      |
                                                             CharacterActionMask.Run       |
                                                             CharacterActionMask.Stop      |
                                                             CharacterActionMask.Stun      |
                                                             CharacterActionMask.KnockBack |
                                                             CharacterActionMask.Skill;
                                                    

        public void Dead()
        {
            if (Conditions.HasFalse) return;
            
            this.RegisterBehaviour(Cb);
            
            OnActivated.Invoke();

            Cb.Pathfinding.Quit();
            Cb.Animating.Dead();
        }
        
        
        private void OnEnable()
        {
            Conditions.Register("OverwriteMask", () => (IgnorableMask | Cb.BehaviourMask) == IgnorableMask);
        }
        
        private void OnDisable()
        {
            Conditions.Unregister("OverwriteMask");
        }
    }
}
