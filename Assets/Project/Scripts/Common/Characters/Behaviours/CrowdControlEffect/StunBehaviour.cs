using UnityEngine;

namespace Common.Characters.Behaviours.CrowdControlEffect
{
    public class StunBehaviour : ActionBehaviour
    {
        public override CharacterActionMask BehaviourMask => CharacterActionMask.Stun;
        public override CharacterActionMask IgnorableMask => CharacterActionMask.StunIgnoreMask;

        public FloatEvent RemainStunTime { get; } = new();
        public ActionTable<float> OnStunning { get; } = new();
        
        protected bool IsAble => Conditions.IsAllTrue 
                                 && CanOverrideToCurrent;


        public void Active(float duration)
        {
            if (!IsAble) return;
            
            RegisterBehaviour(Cb);
            
            OnStunning.Invoke(duration);
            OnActivated.Invoke();
            Cb.Animating.Stun();

            enabled              = true;
            RemainStunTime.Value = duration;
        }

        public override void Cancel()
        {
            enabled              = false;
            RemainStunTime.Value = 0f;

            Cb.Stop();
            OnCanceled.Invoke();
        }

        protected override void Complete()
        {
            enabled              = false;
            RemainStunTime.Value = 0f;
            
            Cb.Stop();
            OnCompleted.Invoke();
        }


        private void Update()
        {
            if (RemainStunTime.Value > 0f)
            {
                RemainStunTime.Value -= Time.deltaTime;
            }
            else
            {
                Complete();
            }
        }
    }
}
