using Core;
using UnityEngine;

namespace Character.Actions
{
    public class CastingAttack : SkillComponent, ICombatTable
    {
        [SerializeField] private PowerValue powerValue;

        public StatTable StatTable { get; } = new();
        
        public void UpdateStatTable() { }

        // public override void Release() { }

        protected override void PlayAnimation()
        {
            CharacterSystem.Animating.PlayLoop(animationKey);
        }
        
        protected virtual void OnCastingAttack()
        {
            if (!CharacterSystem.Colliding.TryGetTakersInSphere(transform.position, range, angle, targetLayer, out var takerList)) return;
            
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
            CharacterSystem.Animating.PlayOnce("attack", 0f, OnEnded.Invoke);
        }


        protected virtual void OnEnable()
        {
            OnActivated.Register("PlayCastingAnimation", PlayAnimation);
            OnActivated.Register("UpdatePowerValue", UpdateCompletion);
            OnActivated.Register("StartProgress", () => StartProgression(OnCompleted.Invoke));

            OnCanceled.Register("EndCallback", OnEnded.Invoke);

            OnCompleted.Register("CastingAttack", OnCastingAttack);
            OnCompleted.Register("PlayEndCastingAnimation", PlayEndCastingAnimation);
            OnCompleted.Register("StartCooling", StartCooling);

            OnEnded.Register("StopProgress", StopProgression);
            OnEnded.Register("Idle", CharacterSystem.Animating.Idle);
        }
        

        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var skillData = MainGame.MainData.SkillSheetData(actionCode);

            powerValue.Value = skillData.CompletionValueList[0];
        }
    }
}
