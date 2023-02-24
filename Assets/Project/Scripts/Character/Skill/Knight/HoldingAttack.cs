 using Core;
 using UnityEngine;

namespace Character.Skill.Knight
{
    public class HoldingAttack : SkillComponent, ICombatTable
    {
        [SerializeField] private PowerValue powerValue;
        
        public StatTable StatTable { get; } = new();
        
        public void UpdateStatTable() { }


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

        private void RegisterHitEvent()
        {
            model.OnHit.Unregister("HoldingHit");
            model.OnHit.Register("HoldingHit", OnHit.Invoke);
        }

        private void OnHoldingAttack()
        {
            if (!colliding.TryGetTakersInSphere(transform.position, range, angle, targetLayer, out var takerList)) return;
            
            takerList.ForEach(taker =>
            {
                taker.TakeDamage(this);
            });
        }

        protected void OnEnable()
        {
            OnActivated.Register("UpdatePowerValue", UpdateCompletion);
            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);

            OnHit.Register("OnHoldingAttack", OnHoldingAttack);
            
            OnCompleted.Register("EndCallback", OnEnded.Invoke);

            OnEnded.Register("StartCooling", StartCooling);
            OnEnded.Register("ReleaseHit", () => model.OnHit.Unregister("HoldingHit"));
        }

        
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var skillData = MainGame.MainData.SkillSheetData(actionCode);

            powerValue.Value = skillData.CompletionValueList[0];
        }
    }
}
