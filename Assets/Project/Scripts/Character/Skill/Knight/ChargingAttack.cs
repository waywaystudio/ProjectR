using Core;
using UnityEngine;

namespace Character.Skill.Knight
{
    public class ChargingAttack : SkillComponent, ICombatTable
    {
        [SerializeField] private PowerValue powerValue;
        
        public StatTable StatTable { get; } = new();

        protected override void UpdateCompletion()
        {
            StatTable.Clear();
            StatTable.Register(ActionCode, powerValue);
            StatTable.UnionWith(Provider.StatTable);
        }

        protected override void PlayAnimation()
        {
            model.PlayLoop(animationKey);
        }
        
        private void OnChargingAttack()
        {
            if (!colliding.TryGetTakersInSphere(transform.position, range, angle, targetLayer, out var takerList)) return;
            
            takerList.ForEach(taker =>
            {
                taker.TakeDamage(this);
                Debug.Log($"{taker.Name} Hit by {ActionCode.ToString()}");
            });
        }

        private void PlayEndChargingAnimation()
        {
            model.PlayOnce("attack", 0f, OnEnded.Invoke);
        }
        
        protected void OnEnable()
        {
            OnActivated.Register("PlayAnimation", PlayAnimation);
            OnActivated.Register("UpdatePowerValue", UpdateCompletion);
            OnActivated.Register("StartProgress", () => StartProcess(OnCompleted.Invoke));
            OnActivated.Register("StartCooling", StartCooling);
            
            OnCompleted.Register("ChargingAttack", OnChargingAttack);
            OnCompleted.Register("PlayEndChargingAnimation", PlayEndChargingAnimation);
            
            OnEnded.Register("StopProgress", StopProcess);
            OnEnded.Register("Idle", model.Idle);
            
            OnInterrupted.Register("Log", () => Debug.Log("Interrupted!"));
        }

        
        public override void SetUp()
        {
            base.SetUp();
            
            var skillData = MainGame.MainData.GetSkill(actionCode);

            powerValue.Value = skillData.CompletionValueList[0];
        }
    }
}
