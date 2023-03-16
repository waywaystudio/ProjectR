using Common.Completion;
using Common.Skills;
using UnityEngine;

namespace Character.Adventurers.Hunter.Skills
{
    public class ContinuesAttack : SkillComponent
    {
        [SerializeField] private PowerCompletion power;
        
        protected override void PlayAnimation()
        {
            Cb.Animating.PlayLoop(animationKey);
        }
        
        private void RegisterHitEvent()
        {
            Cb.Animating.OnHit.Unregister("HoldingHit");
            Cb.Animating.OnHit.Register("HoldingHit", OnHit.Invoke);
        }
        
        
        private void OnHoldingAttack()
        {
            // TODO. 현재 Test상 HitScan 방식이어서 이렇고, Projectile로 바뀌면 교체해야 함.
            var providerTransform = Cb.transform;
            
            if (!Cb.Colliding.TryGetTakersByRaycast(
                    providerTransform.position,
                    providerTransform.forward,
                    range,
                    16,
                    targetLayer,
                    out var takerList)) return;
            
            takerList.ForEach(power.Damage);
        }
        
        protected void OnEnable()
        {
            power.Initialize(Cb, ActionCode);
            
            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);

            OnHit.Register("OnHoldingAttack", OnHoldingAttack);
            
            OnCompleted.Register("EndCallback", OnEnded.Invoke);

            OnEnded.Register("StartCooling", StartCooling);
            OnEnded.Register("ReleaseHit", () => Cb.Animating.OnHit.Unregister("HoldingHit"));
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
