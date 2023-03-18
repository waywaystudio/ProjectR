using Common.Skills;
using UnityEngine;

namespace Character.Adventurers.Rogue.Skills
{
    public class PowerBeat : SkillSequence
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
        }
#endif
    }
}
