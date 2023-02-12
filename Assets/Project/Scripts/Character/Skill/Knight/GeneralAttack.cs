using UnityEngine;

namespace Character.Skill.Knight
{
    public class GeneralAttack : SkillComponent
    {
        protected override void TryActiveSkill()
        {
            if (!ConditionTable.IsAllTrue) return;

            model.OnHit.Unregister("SkillHit");
            model.OnHit.Register("SkillHit", OnHit.Invoke);
            
            OnActivated.Invoke();
        }
        
        protected override void PlayAnimation()
        {
            model.PlayOnce(animationKey, progressTime,
                () =>
                {
                    OnCompleted.Invoke();
                    OnEnded.Invoke();
                });
        }

        private void OnGeneralAttack()
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
            
            OnHit.Register("GeneralAttack", OnGeneralAttack);

            OnEnded.Register("ReleaseHit", () => model.OnHit.Unregister("SkillHit"));
            OnEnded.Register("IdleAnimation", model.Idle);
        }
    }
}
