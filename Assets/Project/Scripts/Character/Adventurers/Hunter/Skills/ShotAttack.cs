using Common.Completion;
using Common.Skills;
using UnityEngine;

namespace Character.Adventurers.Hunter.Skills
{
    public class ShotAttack : SkillSequence
    {
        [SerializeField] private DamageCompletion damage;
        
        protected override void Initialize()
        {
            damage.Initialize(Cb);

            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            OnHit.Register("SwordAttack", OnAttack);
            OnCompleted.Register("EndCallback", End);
            OnEnded.Register("ReleaseHit", UnregisterHitEvent);
        }
        
        public override void OnAttack()
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

            takerList.ForEach(damage.Completion);
        }
        
        private void RegisterHitEvent() => Cb.Animating.OnHit.Register("SkillHit", OnHit.Invoke);
        private void UnregisterHitEvent() => Cb.Animating.OnHit.Unregister("SkillHit");
        

#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var skillData = Database.SkillSheetData(actionCode);

            if (!TryGetComponent(out damage))
            {
                damage = gameObject.AddComponent<DamageCompletion>();
            }

            damage.SetDamage(skillData.CompletionValueList[0]);
        }
#endif
    }
}
