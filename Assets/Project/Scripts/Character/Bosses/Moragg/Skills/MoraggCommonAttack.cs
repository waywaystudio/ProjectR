using Common.Skills;
using UnityEngine;

namespace Character.Bosses.Moragg.Skills
{
    public class MoraggCommonAttack : SkillSequence
    {
        [SerializeField] private DirectHitEvent directHitEvent;
        
        public override void OnAttack()
        {
            if (MainTarget is null) return;

            directHitEvent.Completion(MainTarget);
        }
        

        protected override void Initialize()
        {
            directHitEvent.Initialize();

            OnCompleted.Register("EndCallback", End);
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
