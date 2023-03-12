using Common.Animation;
using Common.Systems;
using UnityEngine;

namespace Common.Actions
{
    public class StopAction : MonoBehaviour, ICharacterAction
    {
        private PathfindingSystem pathfinding;
        private AnimationModel animating;

        public CharacterActionMask ActionType => CharacterActionMask.Stop;
        public CharacterActionMask DisableActionMask => CharacterActionMask.None;
        public ConditionTable Conditions { get; } = new();
        public ActionTable OnActivated { get; } = new();

        
        public void Active()
        {
            if (Conditions.HasFalse) return;
            
            OnActivated.Invoke();

            pathfinding.Stop();
            animating.Idle();
        }


        private void Awake()
        {
            var characterSystem = GetComponentInParent<ICharacterSystem>();

            animating   = characterSystem.Animating;
            pathfinding = characterSystem.Pathfinding;
        }
    }
}
