using Character.StatusEffect;
using Core;
using UnityEngine;
using UnityEngine.Pool;

namespace Character.Skill.Knight
{
    public class ChargingAttack : SkillComponent, ICombatTable
    {
        [SerializeField] private PowerValue powerValue;
        [SerializeField] private StatusEffectPool statusEffectPool;

        public StatTable StatTable { get; } = new();


        protected override void PlayAnimation()
        {
            model.PlayLoop(animationKey);
        }
        
        private void UpdateCompletion()
        {
            StatTable.Clear();
            StatTable.Register(ActionCode, powerValue);
            StatTable.UnionWith(Provider.StatTable);
        }
        
        private void OnChargingAttack()
        {
            if (Provider.Object.TryGetComponent(out ICombatTaker self))
            {
                var effectInfo = statusEffectPool.Effect;
                var table = self.DynamicStatEntry.BuffTable;

                if (table.ContainsKey((Provider, effectInfo.ActionCode)))
                {
                    table[(Provider, effectInfo.ActionCode)].OnOverride();
                }
                else
                {
                    self.TakeBuff(statusEffectPool.Get());
                }
            }

            if (!colliding.TryGetTakersInSphere(transform.position, range, angle, targetLayer, out var takerList)) return;
            
            takerList.ForEach(taker => taker.TakeDamage(this));
        }

        private void PlayEndChargingAnimation()
        {
            model.PlayOnce("attack", 0f, OnEnded.Invoke);
        }

        protected void OnEnable()
        {
            statusEffectPool.Initialize(Provider);
            
            OnActivated.Register("PlayAnimation", PlayAnimation);
            OnActivated.Register("UpdatePowerValue", UpdateCompletion);
            OnActivated.Register("StartProcess", () => StartProcess(OnCompleted.Invoke));
            
            OnInterrupted.Register("Log", () => Debug.Log("Interrupted!"));
            OnInterrupted.Register("EndCallback", OnEnded.Invoke);
            
            OnCompleted.Register("StartCooling", StartCooling);
            OnCompleted.Register("ChargingAttack", OnChargingAttack);
            OnCompleted.Register("PlayEndChargingAnimation", PlayEndChargingAnimation);
            OnCompleted.Register("StopProcess", StopProcess);
            OnCompleted.Register("ProgressEnd", () => OnProgress = false);

            OnEnded.Register("StopProgress", StopProcess);
            OnEnded.Register("Idle", model.Idle);
        }

        
        public override void SetUp()
        {
            base.SetUp();
            
            var skillData = MainGame.MainData.SkillSheetData(actionCode);

            powerValue.Value = skillData.CompletionValueList[0];
        }
    }
}
