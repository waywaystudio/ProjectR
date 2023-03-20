using UnityEngine;

namespace Common.Characters.Behaviours.Movement
{
    public class RunBehaviour : ActionBehaviour
    {
        public override CharacterActionMask BehaviourMask => CharacterActionMask.Run;
        public override CharacterActionMask IgnorableMask => CharacterActionMask.RunIgnoreMask;

        public ActionTable<Vector3> OnDeparting { get; } = new();
        
        protected bool IsAble => Conditions.IsAllTrue 
                                 && Cb.Pathfinding.CanMove 
                                 && CanOverrideToCurrent;

        public void Active(Vector3 destination)
        {
            if (!IsAble) return;
            
            RegisterBehaviour(Cb);
            
            OnDeparting.Invoke(destination);
            OnActivated.Invoke();
        
            Cb.Pathfinding.Move(destination, Complete);
            Cb.Animating.Run();
        }

        public override void Cancel()
        {
            Cb.Stop();
            OnCanceled.Invoke();
        }
        
        protected override void Complete()
        {
            Cb.Stop();
            OnCompleted.Invoke();
        }
    }
}
