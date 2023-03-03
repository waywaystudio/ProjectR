using Character.Graphic;
using Character.Systems;
using Core;
using UnityEngine;

namespace Character.Actions.Commons
{
    public class RotateAction : MonoBehaviour, ICharacterAction
    {
        private PathfindingSystem pathfinding;
        private AnimationModel animating;

        public CharacterActionMask ActionType => CharacterActionMask.Rotate;
        public CharacterActionMask DisableActionMask => CharacterActionMask.None;
        public ConditionTable Conditions { get; } = new();
        public ActionTable<Vector3> OnActivated { get; } = new();
        

        public void Active(Vector3 target)
        {
            if (Conditions.HasFalse) return;
            
            OnActivated.Invoke(target);

            pathfinding.RigidRotate(target);
            animating.Flip(pathfinding.Direction);
        }


        private void Awake()
        {
            var characterSystem = GetComponentInParent<ICharacterSystem>();

            animating   = characterSystem.Animating;
            pathfinding = characterSystem.Pathfinding;
        }
    }
}
