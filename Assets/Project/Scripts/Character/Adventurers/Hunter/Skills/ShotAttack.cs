using Common.Completion;
using Common.Skills;
using UnityEngine;

namespace Adventurers.Hunter.Skills
{
    public class ShotAttack : SkillComponent
    {
        [SerializeField] private PowerCompletion power;
        
        protected override void PlayAnimation()
        {
            Cb.Animating.PlayOnce(animationKey, progressTime, OnCompleted.Invoke);
            // OnCompleted.Invoke();
        }
        
        protected void OnAttack()
        {
            // TODO. 현재 Test상 HitScan 방식이어서 이렇고, Projectile로 바뀌면 교체해야 함.
            var providerTransform = Provider.Object.transform;
            
            if (!Cb.Colliding.TryGetTakersByRaycast(
                    providerTransform.position,
                    providerTransform.forward,
                    range,
                    16,
                    targetLayer,
                    out var takerList)) return;

            takerList.ForEach(power.Damage);
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
