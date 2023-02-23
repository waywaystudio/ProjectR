using Core;
using UnityEngine;

namespace Character.Skill
{
    public class CastingAttack : SkillComponent, ICombatTable
    {
        [SerializeField] private PowerValue powerValue;

        public StatTable StatTable { get; } = new();

        public override void Release() { }

        protected override void PlayAnimation()
        {
            model.PlayLoop(animationKey);
        }
        
        protected virtual void OnCastingAttack()
        {
            if (!colliding.TryGetTakersInSphere(transform.position, range, angle, targetLayer, out var takerList)) return;
            
            takerList.ForEach(taker =>
            {
                taker.TakeDamage(this);
            });
        }
        
        private void UpdateCompletion()
        {
            StatTable.Clear();
            StatTable.Register(ActionCode, powerValue);
            StatTable.UnionWith(Provider.StatTable);
        }

        private void PlayEndCastingAnimation()
        {
            model.PlayOnce("attack", 0f, OnEnded.Invoke);
        }


        protected virtual void OnEnable()
        {
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
