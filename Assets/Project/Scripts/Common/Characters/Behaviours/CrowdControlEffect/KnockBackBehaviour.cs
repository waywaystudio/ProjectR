using UnityEngine;

namespace Common.Characters.Behaviours.CrowdControlEffect
{
    public class KnockBackBehaviour : ActionBehaviour
    {
        public override CharacterActionMask BehaviourMask => CharacterActionMask.Stun;
        public override CharacterActionMask IgnorableMask => CharacterActionMask.None |
                                                             CharacterActionMask.Stop |
                                                             CharacterActionMask.Run  |
                                                             CharacterActionMask.Skill;

        public ActionTable<Vector3, float> OnKnockBacking { get; } = new();
        
        
        public void Active(Vector3 source, float distance)
        {
            if (Conditions.HasFalse) return;
            
            RegisterBehaviour(Cb);
            
            OnKnockBacking.Invoke(source, distance);
            OnActivated.Invoke();
        
            Cb.Pathfinding.KnockBack(source, distance, OnCompleted.Invoke);
            Cb.Animating.Hit();
        }


        private void OnEnable()
        {
            Conditions.Register("OverwriteMask", () => (IgnorableMask | Cb.BehaviourMask) == IgnorableMask);

            OnKnockBacking.Register("Rotate", Cb.Rotate);
            
            OnCompleted.Register("Stop", Cb.Stop);
        }
        
        private void OnDisable()
        {
            Conditions.Unregister("OverwriteMask");

            OnKnockBacking.Unregister("Rotate");
            
            OnCompleted.Unregister("Stop");
        }
    }
}
