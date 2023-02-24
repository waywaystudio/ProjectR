using Character.Move;
using UnityEngine;

namespace Character.Skill.Knight
{
    public class KnightDash : SkillComponent
    {
        [SerializeField] private float dashDistance;
        [SerializeField] private MoveBehaviour move;
        

        private void OnKnightDash()
        {
            if (!MainGame.MainManager.Input.TryGetMousePosition(out var mousePosition)) return;

            move.Dash(mousePosition - transform.position, dashDistance, ()=> OnCompleted.Invoke());
        }
        
        protected void OnEnable()
        {
            OnActivated.Register("PlayAnimation", PlayAnimation);
            OnActivated.Register("OnKnightDash", OnKnightDash);
            OnActivated.Register("StartCooling", StartCooling);
            
            OnCompleted.Register("DirectToEnd", OnEnded.Invoke);
        
            OnEnded.Register("Idle", model.Idle);
        }
        
        
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var skillData = MainGame.MainData.SkillSheetData(actionCode);
        
            dashDistance = skillData.CompletionValueList[0];
        }
    }
}
