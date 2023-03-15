using Common.Completion;
using Common.Skills;
using UnityEngine;

namespace Adventurers.Knight.Skills
{
    public class SwordAttack : SkillComponent
    {
        [SerializeField] private PowerCompletion power;
        
        
        protected override void PlayAnimation()
        {
            Cb.Animating.PlayOnce(animationKey, progressTime, OnCompleted.Invoke);
        }
        
        protected void OnAttack()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;

            takerList.ForEach(taker =>
            {
                power.Damage(taker);
                // armorCrash.Effect(taker);
            });
        }
        
        private void RegisterHitEvent()
        {
            Cb.Animating.OnHit.Register("SkillHit", OnHit.Invoke);
        }
        
        protected void OnEnable()
        {
            power.Initialize(Provider, ActionCode);

            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            
            OnHit.Register("SwordAttack", OnAttack);

            OnCompleted.Register("EndCallback", OnEnded.Invoke);

            OnEnded.Register("ReleaseHit", () => Cb.Animating.OnHit.Unregister("SkillHit"));
        }


#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var skillData = Database.SkillSheetData(actionCode);

            if (!TryGetComponent(out power))
            {
                power = gameObject.AddComponent<PowerCompletion>();
            }

            power.SetPower(skillData.CompletionValueList[0]);
        }
#endif
    }
}
