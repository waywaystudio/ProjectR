using UnityEngine;

namespace Common.Characters.Behaviours.CrowdControlEffect
{
    public class KnockBackBehaviour : ActionBehaviour
    {
        public override CharacterActionMask BehaviourMask => CharacterActionMask.KnockBack;
        public override CharacterActionMask IgnorableMask => CharacterActionMask.KnockBackIgnoreMask;

        public ActionTable<Vector3, float> OnKnockBacking { get; } = new();
        
        protected bool IsAble => Conditions.IsAllTrue 
                                 && CanOverrideToCurrent;
        
        
        public void Active(Vector3 source, float distance)
        {
            if (!IsAble) return;
            
            RegisterBehaviour(Cb);
            
            // KnockBack

            OnKnockBacking.Invoke(source, distance);
            OnActivated.Invoke();
        
            Cb.Rotate(source);
            Cb.Animating.Hit();
            
            // KnockBack
            
            Cb.Pathfinding.KnockBack(source, distance, Complete);
        }
        
        public override void Cancel()
        {
            Cb.Stop();
            OnCanceled.Invoke();
        }

        protected override void Complete()
        {
            // Skill
            // Debug.Log(Cb.BehaviourMask.ToString());
            
            Cb.Stop();
            
            // Debug.Log(Cb.BehaviourMask.ToString());
            OnCompleted.Invoke();
        }
    }
}
