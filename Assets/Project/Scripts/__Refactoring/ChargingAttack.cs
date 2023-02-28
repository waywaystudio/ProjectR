using Character;
using Character.StatusEffect;
using Core;
using UnityEngine;

namespace __Refactoring
{
    public class ChargingAttack : SkillComponent, ICombatTable
    {
        [SerializeField] private PowerValue powerValue;
        [SerializeField] private StatusEffectCompletion drain;

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
        
        private void OnChargingAttack()
        {
            Provider.Object.TryGetComponent(out ICombatTaker self);
            
            drain.Buff(self);

            if (!CharacterSystem.Colliding.TryGetTakersInSphere(this, out var takerList)) return;
            
            takerList.ForEach(taker => taker.TakeDamage(this));
        }

        private void PlayEndChargingAnimation()
        {
            CharacterSystem.Animating.PlayOnce("attack", 0f, OnEnded.Invoke);
        }

        protected void OnEnable()
        {
            OnActivated.Register("UpdatePowerValue", UpdateCompletion);

            OnCompleted.Register("StartCooling", StartCooling);
            OnCompleted.Register("ChargingAttack", OnChargingAttack);
            OnCompleted.Register("PlayEndChargingAnimation", PlayEndChargingAnimation);
            OnCompleted.Register("StopProcess", StopProgression);
            OnCompleted.Register("ProgressEnd", () => IsProgress = false);
        }

        
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var skillData = MainGame.MainData.SkillSheetData(actionCode);

            powerValue.Value = skillData.CompletionValueList[0];
        }
    }
}
