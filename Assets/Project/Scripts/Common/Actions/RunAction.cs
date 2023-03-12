using Common.Animation;
using Common.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Actions
{
    public class RunAction : MonoBehaviour, ICharacterAction
    {
        private PathfindingSystem pathfinding;
        private AnimationModel animating;

        public CharacterActionMask ActionType => CharacterActionMask.Run;
        public CharacterActionMask DisableActionMask => CharacterActionMask.None;
        
        [ShowInInspector]
        public ConditionTable Conditions { get; } = new();
        public ActionTable<Vector3> OnActivated { get; } = new();
        public ActionTable OnCompleted { get; } = new();

        public void Active(Vector3 target)
        {
            if (Conditions.HasFalse) return;
            
            OnActivated.Invoke(target);

            pathfinding.Move(target, OnCompleted.Invoke);
            animating.Run();
        }


        private void Awake()
        {
            var characterSystem = GetComponentInParent<ICharacterSystem>();

            animating   = characterSystem.Animating;
            pathfinding = characterSystem.Pathfinding;
        }

        private void OnEnable()
        {
            Conditions.Register("pathfinding.CanMove", () => pathfinding.CanMove);

            OnCompleted.Register("Idle", animating.Idle);
        }
    }
}
