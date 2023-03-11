using Character.Graphic;
using Character.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character.Actions
{
    public class DeadAction : MonoBehaviour, ICharacterAction
    {
        private PathfindingSystem pathfinding;
        private AnimationModel animating;

        public CharacterActionMask ActionType => CharacterActionMask.Dead;
        public CharacterActionMask DisableActionMask => CharacterActionMask.Run | 
                                                     CharacterActionMask.Rotate | 
                                                     CharacterActionMask.Stop | 
                                                     CharacterActionMask.Skill;

        public ConditionTable Conditions { get; } = new();
        
        [ShowInInspector]
        public ActionTable OnActivated { get; } = new();

        
        public void Active()
        {
            if (Conditions.HasFalse) return;
            
            OnActivated.Invoke();

            pathfinding.Quit();
            animating.Dead();
        }


        private void Awake()
        {
            var characterSystem = GetComponentInParent<ICharacterSystem>();

            animating   = characterSystem.Animating;
            pathfinding = characterSystem.Pathfinding;
        }
    }
}
