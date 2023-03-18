using Common.Skills;
using UnityEngine;

namespace Character.Adventurers.Rogue.Skills
{
    public class Annihilation : SkillSequence
    {
        [SerializeField] private DirectHitEvent directHitEvent;
        
        public override void OnAttack()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;
        
            takerList.ForEach(directHitEvent.Completion);
        }

        protected override void Initialize()
        {
            directHitEvent.Initialize();

            OnActivated.Register("StartCooling", StartCooling);
            OnCompleted.Register("EndCallback", End);
        }

        protected override void Dispose()
        {
            base.Dispose();
            
            directHitEvent.Dispose();
        }
        
        
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();

            TryGetComponent(out directHitEvent);
            
            var skillData = Database.SkillSheetData(actionCode);

            // if (!TryGetComponent(out damage)) damage = gameObject.AddComponent<DamageCompletion>();
            //
            // damage.SetDamage(skillData.CompletionValueList[0]);
        }
#endif
    }
}
