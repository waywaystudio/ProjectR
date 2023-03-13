using Common.Completion;
using Common.Skills;
using UnityEngine;

namespace Adventurers.Knight.Skills
{
    public class Bash : SkillComponent
    {
        [SerializeField] private PowerCompletion power;
        [SerializeField] private StatusEffectCompletion armorCrash;
        
        
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
                armorCrash.DeBuff(taker);
            });
        }
        
        private void RegisterHitEvent()
        {
            CharacterSystem.Animating.OnHit.Register("SkillHit", OnHit.Invoke);
        }
        
        protected void OnEnable()
        {
            power.Initialize(Provider, ActionCode);
            armorCrash.Initialize(Provider);

            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            OnActivated.Register("StartCooling", StartCooling);
            
            OnHit.Register("Bash", OnAttack);

            OnCompleted.Register("EndCallback", OnEnded.Invoke);

            OnEnded.Register("ReleaseHit", () => CharacterSystem.Animating.OnHit.Unregister("SkillHit"));
        }
        
        
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var skillData = Database.SkillSheetData(actionCode);

            if (!TryGetComponent(out power)) power = gameObject.AddComponent<PowerCompletion>();

            power.SetPower(skillData.CompletionValueList[0]);

            Database.StatusEffectMaster.Get((DataIndex)skillData.StatusEffect, out var armorCrashObject);
            
            if (!TryGetComponent(out armorCrash)) armorCrash = gameObject.AddComponent<StatusEffectCompletion>();

            armorCrash.SetProperties(armorCrashObject, 16);
        }
#endif
    }
}
