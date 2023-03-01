using UnityEngine;

namespace Character.Actions.Knight
{
    /* TODO.점프 중에 이동하면 모션이 부자연스러워지는 현상이 있다. 개발 아이디어가 필요하다. */
    // 1. ActionBehaviour.CommonMove.Run(Vector3 destination) 을 bool TryRun으로 바꾼다.
    // 2. ...
    public class ThunderClap : SkillComponent
    {
        [SerializeField] private ValueCompletion power;
        
        
        public override void Release() { }

        protected override void PlayAnimation()
        {
            CharacterSystem.Animating.PlayOnce(animationKey, progressTime, OnCompleted.Invoke);
        }

        private void Jump()
        {
            // TODO. Forward로 바꾼후, 플레이어는 마우스 포지션을 받아, 즉시 회전 후 Forward로 점프하는 형태.
            var dest = !MainGame.MainManager.Input.TryGetMousePosition(out var mousePosition) 
                ? transform.position 
                : mousePosition;
            
            CharacterSystem.Pathfinding.Jump(dest, 11f);
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
            OnActivated.Register("CanNotMoveUntilEnd", () => CharacterSystem.Pathfinding.CanMove = false);
            OnActivated.Register("Jump", Jump);
            OnActivated.Register("StartCooling", StartCooling);
            
            OnHit.Register("ThunderClap", OnAttack);

            OnCompleted.Register("EndCallback", OnEnded.Invoke);

            OnEnded.Register("ReleaseHit", () => CharacterSystem.Animating.OnHit.Unregister("SkillHit"));
            OnEnded.Register("ReleaseMove", () => CharacterSystem.Pathfinding.CanMove = true);
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
