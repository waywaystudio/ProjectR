using Character.StatusEffect;
using Core;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Character.Skill.Knight
{
    public class CastingAttack : SkillComponent, ICombatTable
    {
        [SerializeField] private PowerValue powerValue;
        [SerializeField] private StatusEffectPool statusEffectPool;

        public StatTable StatTable { get; } = new();

        public override void Release() { }

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
        
        private void OnCastingAttack()
        {
            if (!colliding.TryGetTakersInSphere(transform.position, range, angle, targetLayer, out var takerList)) return;
            
            takerList.ForEach(taker =>
            {
                taker.TakeDamage(this);

                var effectInfo = statusEffectPool.Effect;
                var table = taker.DynamicStatEntry.DeBuffTable;

                if (table.ContainsKey((Provider, effectInfo.ActionCode)))
                {
                    table[(Provider, effectInfo.ActionCode)].OnOverride();
                }
                else
                {
                    taker.TakeDeBuff(statusEffectPool.Get());
                }
            });
        }

        private void PlayEndCastingAnimation()
        {
            model.PlayOnce("attack", 0f, OnEnded.Invoke);
        }


        protected void OnEnable()
        {
            statusEffectPool.Initialize(Provider);
            
            OnActivated.Register("PlayCastingAnimation", PlayAnimation);
            OnActivated.Register("UpdatePowerValue", UpdateCompletion);
            OnActivated.Register("StartProgress", () => StartProcess(OnCompleted.Invoke));
            
            OnInterrupted.Register("Log", () => Debug.Log("Interrupted!"));
            OnInterrupted.Register("EndCallback", OnEnded.Invoke);

            OnCompleted.Register("CastingAttack", OnCastingAttack);
            OnCompleted.Register("PlayEndCastingAnimation", PlayEndCastingAnimation);
            OnCompleted.Register("StartCooling", StartCooling);

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
