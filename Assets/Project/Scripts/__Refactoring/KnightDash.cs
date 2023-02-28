using Character.Systems;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Actions.Knight
{
    public class KnightDash : SkillComponent
    {
        [SerializeField] private float dashDistance;
        [SerializeField] private PathfindingSystem pathfinding;
        

        private void OnKnightDash()
        {
            if (!MainGame.MainManager.Input.TryGetMousePosition(out var mousePosition)) return;

            pathfinding.Dash(mousePosition - transform.position, dashDistance, ()=> OnCompleted.Invoke());
        }
        
        protected void OnEnable()
        {
            OnActivated.Register("PlayAnimation", PlayAnimation);
            OnActivated.Register("OnKnightDash", OnKnightDash);
            OnActivated.Register("StartCooling", StartCooling);
            
            OnCompleted.Register("DirectToEnd", OnEnded.Invoke);
        
            OnEnded.Register("Idle", CharacterSystem.Animating.Idle);
        }
        
        
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var skillData = MainGame.MainData.SkillSheetData(actionCode);
        
            dashDistance = skillData.CompletionValueList[0];
        }
    }
}
