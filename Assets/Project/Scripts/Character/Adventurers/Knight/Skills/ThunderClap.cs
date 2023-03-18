using Common.Skills;
using UnityEngine;

namespace Character.Adventurers.Knight.Skills
{
    public class ThunderClap : SkillSequence
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

            OnActivated.Register("Jump", Jump);
            OnActivated.Register("StartCooling", StartCooling);

            OnCompleted.Register("EndCallback", End);
        }

        protected override void Dispose()
        {
            base.Dispose();
            
            directHitEvent.Dispose();
        }
        
        private void Jump()
        {
            var direction = Cb.transform.forward;
            
            Cb.Pathfinding.Jump(direction, 11f, 2.4f, 0.77f);
        }


#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            TryGetComponent(out directHitEvent);
            
            // var skillData = Database.SkillSheetData(actionCode);
            //
            // if (!TryGetComponent(out damage)) damage = gameObject.AddComponent<DamageCompletion>();
            //
            // damage.SetDamage(skillData.CompletionValueList[0]);
        }
#endif
    }
}
