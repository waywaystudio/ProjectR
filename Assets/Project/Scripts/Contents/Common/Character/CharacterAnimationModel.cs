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
        
        public void Initialize(SkeletonAnimation skeletonAnimation)
        {
            this.skeletonAnimation = skeletonAnimation;
        }

        // Look Where;
        [Button] public void LookLeft() => skeletonAnimation.Skeleton.ScaleX = 1.0f;
        [Button] public void LookRight() => skeletonAnimation.Skeleton.ScaleX = -1.0f;
        
        // Do What;
        [Button] public void Idle() => Play("idle", 0, true);
        [Button] public void Attack() => Play("attack", 0, false);
        [Button] public void Walk() => Play("walk", 0, true);
        [Button] public void Run()=> Play("run", 0, true);
        [Button] public void Crouch() => Play("crouch", 0, true);

        public void Flip(Vector3 direction)
        {
            // 추후에는 x, z를 비교하여 캐릭터 방향의 상하좌우를 결정한다.
            // 지금 좌우 뿐이니, direction 의 x값만 알면된다.
            skeletonAnimation.Skeleton.ScaleX = direction.x switch
            {
                < 0 => -1.0f,
                > 0 => 1.0f,
                _ => skeletonAnimation.Skeleton.ScaleX
            };
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
        

        private void Play(string animationKey, int layer, bool loop)
        {
            Play(Animator.StringToHash(animationKey), layer, loop);
        }

        private void Play(int nameHash, int layer, bool loop)
        {
            if (!modelData.TryGetAnimation(nameHash, out var target)) return;
    
            Play(target, layer, loop);
        }
        
        private void Play(Animation target, int layer, bool loop)
        {
            var hasCurrent = TryGetCurrentAnimation(layer, out var current);
            var hasTransition = modelData.TryGetTransition(current, target, out var transition);
        
            if (hasCurrent && hasTransition)
            {
                skeletonAnimation.AnimationState.SetAnimation(layer, transition, false);
                skeletonAnimation.AnimationState.AddAnimation(layer, target, loop, 0f);
    
                return;
            }
            
            skeletonAnimation.AnimationState.SetAnimation(layer, target, loop);
            TargetAnimation = target;
        }
        
        private void Start()
        {
            Idle();
        }
    
        private bool TryGetCurrentAnimation(int layer, out Animation result)
        {
            result = skeletonAnimation.AnimationState.GetCurrent(layer)?.Animation;
    
            return result is not null;
        }
    }
}
