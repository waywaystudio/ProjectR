using Character.StatusEffect;
using UnityEngine;

namespace Character.Skill.Knight
{
    public class KnightCastingAttack : SkillComponent
    {
        [SerializeField] private ValueCompletion power;
        [SerializeField] private StatusEffectCompletion bleed;

        public override void Release() { }
        
        
        protected override void PlayAnimation()
        {
            model.PlayLoop(animationKey);
        }

        private void OnCastingAttack()
        {
            if (!colliding.TryGetTakersInSphere(this, out var takerList)) return;
            
            takerList.ForEach(taker =>
            {
                power.Damage(taker);
                bleed.Effect(taker);
            });
        }
        
        private void PlayEndCastingAnimation()
        {
            model.PlayOnce("attack", 0f, OnEnded.Invoke);
        }
        
        protected void OnEnable()
        {
            power.Initialize(Provider, ActionCode);

            OnCompleted.Register("CastingAttack", OnCastingAttack);
            OnCompleted.Register("PlayEndCastingAnimation", PlayEndCastingAnimation);
            OnCompleted.Register("StartCooling", StartCooling);
        }
        
        
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var skillData = MainGame.MainData.SkillSheetData(actionCode);

            power.SetPower(skillData.CompletionValueList[0]);
        }
    }
}
