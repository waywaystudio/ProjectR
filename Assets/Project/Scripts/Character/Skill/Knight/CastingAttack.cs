using UnityEngine;

namespace Character.Skill.Knight
{
    public class CastingAttack : SkillComponent
    {
        protected override void PlayAnimation()
        {
            model.Play(animationKey);
        }
        
        private void OnCastingAttack()
        {
            if (!targeting.TryGetTargetList(transform.position, out var takerList)) return;
            
            takerList.ForEach(taker =>
            {
                taker.TakeDamage(this);
                Debug.Log($"{taker.Name} Hit by {ActionCode.ToString()}");
            });
        }

        private void PlayEndCastingAnimation()
        {
            model.Play("attack", 0, false, 0f, model.Idle);
        }
        
        protected void OnEnable()
        {
            OnActivated.Register("PlayCastingAnimation", PlayAnimation);
            OnActivated.Register("UpdatePowerValue", UpdatePowerValue);
            OnActivated.Register("StartProgress", () => StartProgress(OnCompleted.Invoke));
            OnActivated.Register("StartCooling", StartCooling);
            OnCompleted.Register("CastingAttack", OnCastingAttack);
            OnCompleted.Register("PlayEndCastingAnimation", PlayEndCastingAnimation);
            OnEnded.Register("StopProgress", StopProcess);
        }
    }
}
