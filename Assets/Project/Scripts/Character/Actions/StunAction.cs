using System.Collections;
using Character.Graphic;
using Character.Systems;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character.Actions
{
    public class StunAction : MonoBehaviour, ICharacterAction
    {
        private PathfindingSystem pathfinding;
        private AnimationModel animating;
        private Coroutine stunRoutine;
        private float duration;

        public CharacterActionMask ActionType => CharacterActionMask.Stun;
        public CharacterActionMask DisableActionMask => 
            CharacterActionMask.Run | 
            CharacterActionMask.Rotate;
        
        [ShowInInspector]
        public ConditionTable Conditions { get; } = new();
        public ActionTable<float> OnActivated { get; } = new();
        public ActionTable OnCompleted { get; } = new();

        public void Active(float duration)
        {
            if (Conditions.HasFalse) return;
            
            OnActivated.Invoke(duration);
            
            StartStunRoutine(duration);
            animating.Stun();
        }
        

        private void StartStunRoutine(float duration)
        {
            if (stunRoutine != null)
            {
                this.duration = duration;
            }
            else
            {
                stunRoutine = StartCoroutine(Stunning(duration));
            }
        }
        
        private IEnumerator Stunning(float duration)
        {
            this.duration = duration;
            
            while (this.duration > 0f)
            {
                this.duration -= Time.deltaTime;

                yield return null;
            }
            
            OnCompleted.Invoke();
        }
        
        private void StopStunRoutine()
        {
            StopCoroutine(stunRoutine);
            stunRoutine = null;
        }


        private void Awake()
        {
            var characterSystem = GetComponentInParent<ICharacterSystem>();

            animating   = characterSystem.Animating;
            pathfinding = characterSystem.Pathfinding;
        }

        private void OnEnable()
        {
            OnCompleted.Register("StopRoutine", StopStunRoutine);
            OnCompleted.Register("Idle", animating.Idle);
        }
    }
}
