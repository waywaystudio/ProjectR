using Character.Graphic;
using Character.Systems;
using UnityEngine;

namespace Character.Actions
{
    public class KnockBackAction : MonoBehaviour, ICharacterAction
    {
        private PathfindingSystem pathfinding;
        private AnimationModel animating;
        
        public CharacterActionMask ActionType => CharacterActionMask.Stun;
        public CharacterActionMask DisableActionMask => 
            CharacterActionMask.Run | 
            CharacterActionMask.Rotate |
            CharacterActionMask.Skill;
        
        public ConditionTable Conditions { get; } = new();
        public ActionTable<Vector3, float> OnActivated { get; } = new();
        public ActionTable OnCompleted { get; } = new();
        
        public void Active(Vector3 source, float distance)
        {
            if (Conditions.HasFalse) return;
            
            OnActivated.Invoke(source, distance);
            
            pathfinding.KnockBack(source, distance, OnCompleted.Invoke);
            animating.Stun();
        }
        
        
        private void Awake()
        {
            var characterSystem = GetComponentInParent<ICharacterSystem>();

            animating   = characterSystem.Animating;
            pathfinding = characterSystem.Pathfinding;
        }

        private void OnEnable()
        {
            OnCompleted.Register("Idle", animating.Idle);
        }
    }
}
