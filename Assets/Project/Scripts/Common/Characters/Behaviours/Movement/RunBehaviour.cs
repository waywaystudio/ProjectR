using UnityEngine;

namespace Common.Characters.Behaviours.Movement
{
    public class RunBehaviour : ActionBehaviour
    {
        public override CharacterActionMask BehaviourMask => CharacterActionMask.Run;
        public override CharacterActionMask IgnorableMask => CharacterActionMask.None |
                                                             CharacterActionMask.Stop |
                                                             CharacterActionMask.Run  |
                                                             CharacterActionMask.Skill;
            

        public ActionTable<Vector3> OnDeparting { get; } = new();
        
        
        public void Active(Vector3 destination)
        {
            if (Conditions.HasFalse) return;
            
            RegisterBehaviour(Cb);
            
            OnDeparting.Invoke(destination);
            OnActivated.Invoke();
        
            Cb.Pathfinding.Move(destination, OnCompleted.Invoke);
            Cb.Animating.Run();
        }


        private void OnEnable()
        {
            Conditions.Register("pathfinding.CanMove", () => Cb.Pathfinding.CanMove);
            Conditions.Register("OverwriteMask", IsOverBehaviour);
            
            OnCanceled.Register("Stop", Cb.Stop);

            OnCompleted.Register("Stop", Cb.Stop);
        }
        
        private void OnDisable()
        {
            Conditions.Unregister("pathfinding.CanMove");
            Conditions.Unregister("OverwriteMask");
            
            OnCanceled.Unregister("Stop");
            
            OnCompleted.Unregister("Stop");
        }
    }
}
