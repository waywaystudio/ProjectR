using UnityEngine;

namespace Character.Actions.Knight
{
    /* TODO.점프 중에 이동하면 모션이 부자연스러워지는 현상이 있다. 개발 아이디어가 필요하다. */
    // 1. ActionBehaviour.CommonMove.Run(Vector3 destination) 을 bool TryRun으로 바꾼다.
    // 2. ...
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
            
            CharacterSystem.Pathfinding.Jump(direction, 11f);
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
            
            var skillData = MainGame.MainData.SkillSheetData(actionCode);

            if (!TryGetComponent(out power)) power = gameObject.AddComponent<ValueCompletion>();

            power.SetPower(skillData.CompletionValueList[0]);
        }
#endif
    }
}
