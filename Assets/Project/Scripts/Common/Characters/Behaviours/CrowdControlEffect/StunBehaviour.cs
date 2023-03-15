using System.Collections;
using UnityEngine;

namespace Common.Characters.Behaviours.CrowdControlEffect
{
    public class StunBehaviour : ActionBehaviour
    {
        private Coroutine stunRoutine;
        private float duration;
        
        public override CharacterActionMask BehaviourMask => CharacterActionMask.Stun;
        public override CharacterActionMask IgnorableMask => CharacterActionMask.None |
                                                             CharacterActionMask.Stop |
                                                             CharacterActionMask.Run  |
                                                             CharacterActionMask.Skill ;
            

        public ActionTable<float> OnStunning { get; } = new();


        public void Active(float duration)
        {
            if (Conditions.HasFalse) return;
            
            this.RegisterBehaviour(Cb);
            
            OnStunning.Invoke(duration);
            OnActivated.Invoke();
            Cb.Animating.Stun();
        }
        
        
        private void StartStunRoutine(float duration)
        {
            if (stunRoutine != null)
            {
                this.duration = duration;
            }
            else
            {
                stunRoutine = StartCoroutine(Stunning(duration));
            }
        }
        
        private IEnumerator Stunning(float duration)
        {
            this.duration = duration;
            
            while (this.duration > 0f)
            {
                this.duration -= Time.deltaTime;

                yield return null;
            }
            
            OnCompleted.Invoke();
        }
        
        private void StopStunRoutine()
        {
            StopCoroutine(stunRoutine);
            stunRoutine = null;
        }

        private void OnEnable()
        {
            Conditions.Register("OverwriteMask", () => (IgnorableMask | Cb.BehaviourMask) == IgnorableMask);
            
            OnStunning.Register("StartStunRoutine", StartStunRoutine);
            
            OnCompleted.Register("StopRoutine", StopStunRoutine);
            OnCompleted.Register("Stop", Cb.Stop);
        }

        private void OnDisable()
        {
            Conditions.Unregister("OverwriteMask");
            
            OnStunning.Unregister("StartStunRoutine");
            
            OnCompleted.Unregister("StopRoutine");
            OnCompleted.Unregister("Stop");
        }
    }
}