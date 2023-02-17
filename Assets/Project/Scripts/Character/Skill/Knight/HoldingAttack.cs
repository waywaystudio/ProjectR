 using Core;
 using UnityEngine;

namespace Character.Skill.Knight
{
    public class HoldingAttack : SkillComponent, ICombatTable
    {
        [SerializeField] private PowerValue powerValue;
        
        public StatTable StatTable { get; } = new();


        public override void Release()
        {
            if (!OnProgress) return;
            
            OnCompleted.Invoke();
        }
        
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
        
        protected override void TryActiveSkill()
        {
            if (!ConditionTable.IsAllTrue) return;

            model.OnHit.Unregister("HoldingHit");
            model.OnHit.Register("HoldingHit", OnHit.Invoke);
            
            OnActivated.Invoke();
        }

        private void OnHoldingAttack()
        {
            if (!colliding.TryGetTakersInSphere(transform.position, range, angle, targetLayer, out var takerList)) return;
            
            takerList.ForEach(taker =>
            {
                taker.TakeDamage(this);
                Debug.Log($"{taker.Name} Hit by {ActionCode.ToString()}");
            });
        }

        protected void OnEnable()
        {
            OnActivated.Register("PlayAnimation", PlayAnimation);
            OnActivated.Register("UpdatePowerValue", UpdateCompletion);
            OnActivated.Register("StartProgress", () => StartProcess(OnEnded.Invoke));
            OnActivated.Register("StartCooling", StartCooling);

            OnHit.Register("OnHoldingAttack", OnHoldingAttack);
            
            OnEnded.Register("ReleaseHit", () => model.OnHit.Unregister("HoldingHit"));
            OnEnded.Register("StopProgress", StopProcess);
            OnEnded.Register("Idle", model.Idle);
            
            OnInterrupted.Register("Log", () => Debug.Log("Interrupted"));
        }

        
        public override void SetUp()
        {
            base.SetUp();
            
            var skillData = MainGame.MainData.SkillSheetData(actionCode);

            powerValue.Value = skillData.CompletionValueList[0];
        }
    }
}
