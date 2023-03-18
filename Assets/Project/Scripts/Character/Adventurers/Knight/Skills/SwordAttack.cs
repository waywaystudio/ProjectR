using Common.Skills;
using UnityEngine;

namespace Character.Adventurers.Knight.Skills
{
    public class SwordAttack : SkillSequence
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
        }
#endif
    }
}
