using Character.StatusEffect;
using UnityEngine;

namespace Character.Skill.Knight
{
    public class KnightCommonAttack : SkillComponent
    {
        [SerializeField] protected ValueCompletion power;
        [SerializeField] private StatusEffectCompletion armorCrash;

        public override void Release() { }
        
        protected override void PlayAnimation()
        {
            model.PlayOnce(animationKey, progressTime, OnCompleted.Invoke);
        }
        
        protected void OnAttack()
        {
            if (!colliding.TryGetTakersInSphere(this, out var takerList)) return;

            takerList.ForEach(taker =>
            {
                power.Damage(taker);
                armorCrash.Effect(taker);
            });
        }
        
        private void RegisterHitEvent()
        {
            model.OnHit.Register("SkillHit", OnHit.Invoke);
        }
        
        protected void OnEnable()
        {
            power.Initialize(Provider, ActionCode);

            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            
            OnHit.Register("GeneralAttack", OnAttack);

            OnCompleted.Register("EndCallback", OnEnded.Invoke);

            OnEnded.Register("ReleaseHit", () => model.OnHit.Unregister("SkillHit"));
        }
        
        
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var skillData = MainGame.MainData.SkillSheetData(actionCode);

            power.SetPower(skillData.CompletionValueList[0]);
        }
    }
}
