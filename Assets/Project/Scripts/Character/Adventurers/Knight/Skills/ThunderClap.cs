using Common.Completion;
using Common.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Adventurers.Knight.Skills
{
    public class ThunderClap : SkillComponent
    {
        [FormerlySerializedAs("power")] [SerializeField] private DamageCompletion damage;
        

        private void Jump()
        {
            var direction = Cb.transform.forward;
            
            Cb.Pathfinding.Jump(direction, 11f, 2.4f, 0.77f);
        }
        
        protected void OnAttack()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;

            takerList.ForEach(taker =>
            {
                damage.Damage(taker);
            });
        }
        
        protected override void Initialize()
        {
            damage.Initialize(Cb, ActionCode);

            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            OnActivated.Register("Jump", Jump);
            OnActivated.Register("StartCooling", StartCooling);
            
            OnHit.Register("ThunderClap", OnAttack);
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

            if (!TryGetComponent(out damage)) damage = gameObject.AddComponent<DamageCompletion>();

            damage.SetDamage(skillData.CompletionValueList[0]);
        }
#endif
    }
}
