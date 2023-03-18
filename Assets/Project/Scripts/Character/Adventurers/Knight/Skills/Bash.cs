using Common.Skills;
using UnityEngine;
// SkillMechanicEntity;

namespace Character.Adventurers.Knight.Skills
{
    public class Bash : SkillSequence
    {
        [SerializeField] private DirectHitEvent directHitEvent;

        protected override void Initialize()
        {
            directHitEvent.Initialize();

            OnActivated.Register("StartCooling", StartCooling);
            OnCompleted.Register("EndCallback", End);
        }
        
        public override void OnAttack()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;
        
            takerList.ForEach(directHitEvent.Completion);
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
        }
#endif
    }
}
