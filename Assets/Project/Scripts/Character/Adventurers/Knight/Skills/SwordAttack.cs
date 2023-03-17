using Common.Completion;
using Common.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Adventurers.Knight.Skills
{
    public class SwordAttack : SkillComponent
    {
        [FormerlySerializedAs("power")] [SerializeField] private DamageCompletion damage;
        

        protected void OnAttack()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;

            takerList.ForEach(taker => damage.Damage(taker));
        }
        
        protected override void Initialize()
        {
            damage.Initialize(Cb, ActionCode);

            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            OnHit.Register("SwordAttack", OnAttack);
            OnCompleted.Register("EndCallback", End);
            OnEnded.Register("ReleaseHit", UnregisterHitEvent);
        }

        protected override void Dispose()
        {
            // TODO. Unregister Sequence Events;
        }


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
