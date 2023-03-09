using Core;
using UnityEngine;

namespace Character.Actions.Knight
{
    public class HoldingAttack : SkillComponent, ICombatTable
    {
        [SerializeField] private PowerValue powerValue;
        
        public StatTable StatTable { get; } = new();
        
        public void UpdateStatTable() { }


        protected override void PlayAnimation()
        {
            CharacterSystem.Animating.PlayLoop(animationKey);
        }
        
        private void UpdateCompletion()
        {
            StatTable.Clear();
            StatTable.Register(ActionCode, powerValue);
            StatTable.UnionWith(Provider.StatTable);
        }

        private void RegisterHitEvent()
        {
            CharacterSystem.Animating.OnHit.Unregister("HoldingHit");
            CharacterSystem.Animating.OnHit.Register("HoldingHit", OnHit.Invoke);
        }

        private void OnHoldingAttack()
        {
            if (!CharacterSystem.Colliding.TryGetTakersInSphere(transform.position, range, angle, targetLayer, out var takerList)) return;
            
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
            OnEnded.Register("ReleaseHit", () => CharacterSystem.Animating.OnHit.Unregister("HoldingHit"));
        }

        
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var skillData = DB.SkillSheetData(actionCode);

            powerValue.Value = skillData.CompletionValueList[0];
        }
    }
}
