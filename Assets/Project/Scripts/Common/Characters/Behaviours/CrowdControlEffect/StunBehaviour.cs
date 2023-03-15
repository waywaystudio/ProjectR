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
            
            RegisterBehaviour(Cb);
            
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
            Conditions.Register("OverwriteMask", IsOverBehaviour);
            
            OnStunning.Register("StartStunRoutine", StartStunRoutine);
            
            OnCanceled.Register("StopRoutine", StopStunRoutine);
            OnCanceled.Register("Stop", Cb.Stop);
            
            OnCompleted.Register("StopRoutine", StopStunRoutine);
            OnCompleted.Register("Stop", Cb.Stop);
        }

        private void OnDisable()
        {
            Conditions.Unregister("OverwriteMask");
            
            OnStunning.Unregister("StartStunRoutine");
            
            OnCanceled.Unregister("StopRoutine");
            OnCanceled.Unregister("Stop");
            
            OnCompleted.Unregister("StopRoutine");
            OnCompleted.Unregister("Stop");
        }
    }
}
