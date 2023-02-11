using UnityEngine;

namespace Character.Skill.Knight
{
    public class HoldingAttack : SkillComponent
    {
        protected override void PlayAnimation()
        {
            model.Play(animationKey);
        }
        
        protected override void TryActiveSkill()
        {
            if (!ConditionTable.IsAllTrue) return;

            model.OnHit.Unregister("SkillHit");
            model.OnHit.Register("SkillHit", OnHit.Invoke);
            
            OnActivated.Invoke();
        }

        private void OnHoldingAttack()
        {
            if (!targeting.TryGetTargetList(transform.position, out var takerList)) return;
            
            takerList.ForEach(taker =>
            {
                taker.TakeDamage(this);
                Debug.Log($"{taker.Name} Hit by {ActionCode.ToString()}");
            });
        }

        protected void OnEnable()
        {
            OnActivated.Register("PlayAnimation", PlayAnimation);
            OnActivated.Register("UpdatePowerValue", UpdatePowerValue);
            OnActivated.Register("StartProgress", () => StartProgress(Complete));
            OnActivated.Register("StartCooling", StartCooling);

            OnHit.Register("OnChannelingAttack", OnHoldingAttack);
            
            OnEnded.Register("ReleaseHit", () => model.OnHit.Unregister("SkillHit"));
            OnEnded.Register("StopProgress", StopProcess);
            OnEnded.Register("IdleAnimation", model.Idle);
        }
    }
}
