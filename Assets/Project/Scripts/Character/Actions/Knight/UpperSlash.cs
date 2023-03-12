using Character.StatusEffect;
using UnityEngine;

namespace Character.Actions.Knight
{
    public class UpperSlash : SkillComponent
    {
        [SerializeField] private ValueCompletion power;
        [SerializeField] private StatusEffectCompletion bleed;
        
        
        protected override void PlayAnimation()
        {
            CharacterSystem.Animating.PlayOnce(animationKey, progressTime, OnCompleted.Invoke);
        }
        
        protected void OnAttack()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;

            takerList.ForEach(taker =>
            {
                power.Damage(taker);
                bleed.DeBuff(taker);
            });
        }
        
        private void RegisterHitEvent()
        {
            CharacterSystem.Animating.OnHit.Register("SkillHit", OnHit.Invoke);
        }
        
        protected void OnEnable()
        {
            power.Initialize(Provider, ActionCode);
            bleed.Initialize(Provider);

            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            OnActivated.Register("StartCooling", StartCooling);
            
            OnHit.Register("UpperSlash", OnAttack);

            OnCompleted.Register("EndCallback", OnEnded.Invoke);

            OnEnded.Register("ReleaseHit", () => CharacterSystem.Animating.OnHit.Unregister("SkillHit"));
        }
        
        
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var skillData = Database.SkillSheetData(actionCode);

            if (!TryGetComponent(out power)) power = gameObject.AddComponent<ValueCompletion>();

            power.SetPower(skillData.CompletionValueList[0]);

            Database.StatusEffectMaster.Get((DataIndex)skillData.StatusEffect, out var armorCrashObject);
            
            if (!TryGetComponent(out bleed)) bleed = gameObject.AddComponent<StatusEffectCompletion>();

            bleed.SetProperties(armorCrashObject, 16);
        }
#endif
    }
}
