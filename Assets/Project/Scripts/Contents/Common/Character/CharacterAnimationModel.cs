using System;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;
using Animation = Spine.Animation;

namespace Common.Character
{
    public class CharacterAnimationModel : MonoBehaviour
    {
        [SerializeField] private AnimationModelData modelData;
        
        private SkeletonAnimation skeletonAnimation;
        private Spine.AnimationState state;

        public Animation TargetAnimation { get; private set; }

        // Look Where;
        public void LookLeft() => skeletonAnimation.Skeleton.ScaleX = 1.0f;
        public void LookRight() => skeletonAnimation.Skeleton.ScaleX = -1.0f;
        
        // Do What;
        public void Idle(bool loop = true, Action callback = null) => Play("idle", 0, loop, callback);
        public void Attack(bool loop = true, Action callback = null) => Play("attack", 0, loop, callback);
        public void Walk(bool loop = true, Action callback = null) => Play("walk", 0, loop, callback);
        public void Run(bool loop = true, Action callback = null)=> Play("run", 0, loop, callback);
        public void Crouch(bool loop = true, Action callback = null) => Play("crouch", 0, loop, callback);

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

        private void PlayOneShot(string animationKey, int layer)
        {
            if (!modelData.TryGetAnimation(animationKey, out var target))
            {
                Debug.LogError($"Not Exist Animation Key {animationKey}");
                return;
            }

            PlayOneShot(target, layer);
        }

        private void PlayOneShot(Animation oneShot, int layer)
        {
            state.SetAnimation(0, oneShot, false);
    
            if (modelData.TryGetTransition(oneShot, TargetAnimation, out var transition))
            {
                state.AddAnimation(0, transition, false, 0f);
            }
    
            state.AddAnimation(0, TargetAnimation, true, 0f);
        }

        private void Play(string animationKey, int layer, bool loop = true, Action callback = null)
        {
            if (!modelData.TryGetAnimation(animationKey, out var target))
            {
                Debug.LogError($"Not Exist Animation Key {animationKey}");
                return;
            }
            
            Play(target, layer, loop, callback);
        }

        private void Play(Animation target, int layer, bool loop, Action callback = null)
        {
            TrackEntry entry;
            
            if (target.Equals(TargetAnimation) && loop)
            {
                entry = state.GetCurrent(layer);
                
                if (callback != null) 
                    entry.Complete += _ => callback.Invoke();
                
                return;
            }
            
            var hasCurrent = TryGetCurrentAnimation(layer, out var current);
            var hasTransition = modelData.TryGetTransition(current, target, out var transition);

            if (hasCurrent && hasTransition)
            {
                entry = state.SetAnimation(layer, transition, false);

                if (callback != null) 
                    entry.Complete += _ => callback.Invoke();
                
                state.AddAnimation(layer, target, loop, 0f);
    
                return;
            }

            entry = state.SetAnimation(layer, target, loop);

            if (callback != null) 
                entry.Complete += _ => callback.Invoke();

            TargetAnimation = target;
        }

        private void Awake()
        {
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            state = skeletonAnimation.AnimationState;
        }

        private void OnEnable()
        {
            Idle();
        }
    
        private bool TryGetCurrentAnimation(int layer, out Animation result)
        {
            result = state.GetCurrent(layer)?.Animation;
    
            return result is not null;
        }
    }
}
