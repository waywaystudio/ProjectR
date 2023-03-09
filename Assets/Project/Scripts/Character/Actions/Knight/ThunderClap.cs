using UnityEngine;

namespace Character.Actions.Knight
{
    public class ThunderClap : SkillComponent
    {
        [SerializeField] private ValueCompletion power;
        

        protected override void PlayAnimation()
        {
            CharacterSystem.Animating.PlayOnce(animationKey, progressTime, OnCompleted.Invoke);
        }

        private void Jump()
        {
            var direction = Provider.Object.transform.forward;
            
            CharacterSystem.Pathfinding.Jump(direction, 11f, 2.4f, 0.77f);
        }
        
        protected void OnAttack()
        {
            if (!CharacterSystem.Colliding.TryGetTakersInSphere(this, out var takerList)) return;

            takerList.ForEach(taker =>
            {
                power.Damage(taker);
            });
        }
        
        private void RegisterHitEvent()
        {
            CharacterSystem.Animating.OnHit.Register("SkillHit", OnHit.Invoke);
        }
        
        protected void OnEnable()
        {
            power.Initialize(Provider, ActionCode);

            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            OnActivated.Register("Jump", Jump);
            OnActivated.Register("StartCooling", StartCooling);
            
            OnHit.Register("ThunderClap", OnAttack);

            OnCompleted.Register("EndCallback", OnEnded.Invoke);

            OnEnded.Register("ReleaseHit", () => CharacterSystem.Animating.OnHit.Unregister("SkillHit"));
        }
        
        
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var skillData = DB.SkillSheetData(actionCode);

            if (!TryGetComponent(out power)) power = gameObject.AddComponent<ValueCompletion>();

            power.SetPower(skillData.CompletionValueList[0]);
        }
#endif
    }
}
