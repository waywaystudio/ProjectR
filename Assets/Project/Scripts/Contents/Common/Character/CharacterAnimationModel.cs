using Core;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using Animation = Spine.Animation;

namespace Common.Character
{
    public class CharacterAnimationModel : MonoBehaviour
    {
        [SerializeField] private SkeletonAnimation skeletonAnimation;
        [SerializeField] private AnimationModelData modelData;

        public Animation TargetAnimation { get; private set; }

        public void OnStateChange(CharacterState state)
        {
            var key = state.ToString().ToCamelCase();
            
            Play(key, 0);
        }
        
        // Look Where;
        [Button] public void LookLeft() => skeletonAnimation.Skeleton.ScaleX = 1.0f;
        [Button] public void LookRight() => skeletonAnimation.Skeleton.ScaleX = -1.0f;
        
        // Do What;
        [Button] public void Idle() => Play("idle", 0);
        [Button] public void Attack() => Play("attack", 0);
        [Button] public void Walk() => Play("walk", 0);
        [Button] public void Run() => Play("run", 0);
        [Button] public void Crouch() => Play("crouch", 0);

        public void SetDirection(Vector2 input)
        {
            skeletonAnimation.Skeleton.ScaleX = input.x switch
            {
                < 0 => -1.0f,
                > 0 => 1.0f,
                _ => skeletonAnimation.Skeleton.ScaleX
            };
        }

        public void Initialize(SkeletonAnimation skeletonAnimation)
        {
            this.skeletonAnimation = skeletonAnimation;
        }
    
        private void Start()
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
        }

        private void Play(string animationKey, int layer) => Play(Animator.StringToHash(animationKey), layer);
        private void Play(int nameHash, int layer)
        {
            if (!modelData.TryGetAnimation(nameHash, out var target)) return;
    
            Play(target, layer);
        }
        private void Play(Animation target, int layer)
        {
            var hasCurrent = TryGetCurrentAnimation(layer, out var current);
            var hasTransition = modelData.TryGetTransition(current, target, out var transition);
        
            if (hasCurrent && hasTransition)
            {
                skeletonAnimation.AnimationState.SetAnimation(layer, transition, false);
                skeletonAnimation.AnimationState.AddAnimation(layer, target, true, 0f);
    
                return;
            }
        
            skeletonAnimation.AnimationState.SetAnimation(layer, target, true);
            TargetAnimation = target;
        }
    
        public void PlayOneShot(Animation oneShot, int layer)
        {
            var state = skeletonAnimation.AnimationState;
    
            state.SetAnimation(0, oneShot, false);
    
            if (modelData.TryGetTransition(oneShot, TargetAnimation, out var transition))
            {
                state.AddAnimation(0, transition, false, 0f);
            }
    
            state.AddAnimation(0, TargetAnimation, true, 0f);
        }
    
        private bool TryGetCurrentAnimation(int layer, out Animation result)
        {
            result = skeletonAnimation.AnimationState.GetCurrent(layer)?.Animation;
    
            return result is not null;
        }
    }
}
