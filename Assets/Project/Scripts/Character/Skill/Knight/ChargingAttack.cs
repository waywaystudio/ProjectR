using UnityEngine;

namespace Character.Skill.Knight
{
    public class ChargingAttack : SkillComponent
    {
        protected override void PlayAnimation()
        {
            model.Play(animationKey);
        }
        
        private void OnChargingAttack()
        {
            if (!targeting.TryGetTargetList(transform.position, out var takerList)) return;
            
            takerList.ForEach(taker =>
            {
                taker.TakeDamage(this);
                Debug.Log($"{taker.Name} Hit by {ActionCode.ToString()}");
            });
        }

        private void PlayEndChargingAnimation()
        {
            model.Play("attack", 0, false);
        }
        
        protected void OnEnable()
        {
            OnActivated.Register("PlayAnimation", PlayAnimation);
            OnActivated.Register("UpdatePowerValue", UpdatePowerValue);
            OnActivated.Register("StartProgress", () => StartProgress());
            OnActivated.Register("StartCooling", StartCooling);
            
            OnCompleted.Register("ChargingAttack", OnChargingAttack);
            OnCompleted.Register("PlayEndChargingAnimation", PlayEndChargingAnimation);
            
            OnEnded.Register("StopProgress", StopProcess);
            OnEnded.Register("Idle", model.Idle);
            
            OnInterrupted.Register("Log", () => Debug.Log("Interrupted!"));
        }
    }
}
