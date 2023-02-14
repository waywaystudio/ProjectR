using System;
using Core;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;
using SpineAnimation = Spine.Animation;
using SpineState = Spine.AnimationState;
using Event = Spine.Event;

namespace Character.Graphic
{
    public class AnimationModel : MonoBehaviour
    {
        [SerializeField] private AnimationModelData modelData;
        [SerializeField] private SkeletonAnimation skeletonAnimation;
        
        private SpineState state;
        private TrackEntry entryBuffer;
        
        [ShowInInspector]
        public ActionTable OnHit { get; set; } = new();
        private SpineAnimation TargetAnimation { get; set; }
        
        
        public void Idle()
        {
            state.TimeScale = 1f;
            
            PlayLoop("idle");
        }
        public void Walk() => PlayLoop("walk");
        public void Run() => PlayLoop("run");
        public void Dead() => PlayOnce("Dead");

        public void PlayOnce(string animationKey, float timeScale = 0f, Action callback = null) 
            => Play(animationKey, 0, false, timeScale, callback);
        public void PlayLoop(string animationKey, float timeScale = 0f, Action callback = null) 
            => Play(animationKey, 0, true, timeScale, callback);

        public void Play(string animationKey, int layer, bool loop, float timeScale, Action callback)
        {
            if (!modelData.TryGetAnimation(animationKey, out var target))
            {
                Debug.LogError($"Not Exist Animation Key {animationKey}");
                return;
            }
            
            entryBuffer = null;

            if (IsSameAnimation(target, layer, loop)) return;
            if (HasTransition(target, layer, loop, callback)) return;

            entryBuffer = state.SetAnimation(layer, target, loop);

            // Assign Custom timeScale Animation Speed
            if (timeScale != 0f)
            {
                var originalDuration = target.Duration;
                var toStaticValue = originalDuration / timeScale;

                state.TimeScale      *= toStaticValue;
                entryBuffer.Complete += _ => state.TimeScale = 1f;
                // state.TimeScale /= toStaticValue;
            }
            
            // Handle Callback
            if (callback != null) 
                entryBuffer.Complete += _ => callback.Invoke();

            TargetAnimation = target;
        }

        public void Flip(Vector3 direction)
        {
            skeletonAnimation.Skeleton.ScaleX = direction.x switch
            {
                < 0 => -1.0f,
                > 0 => 1.0f,
                _ => skeletonAnimation.Skeleton.ScaleX
            };
        }
        

        private bool IsSameAnimation(SpineAnimation target, int layer, bool loop)
        {
            if (!target.Equals(TargetAnimation) || !loop) return false;
            
            entryBuffer = state.GetCurrent(layer);

            return true;
        }

        private bool HasTransition(SpineAnimation target, int layer, bool loop, Action callback)
        {
            // TODO. 현재 Animation A에서 B로 넘어갈때 트랜지션 Animation이 따로 있는 경우가 없다.
            var hasCurrent = TryGetCurrentAnimation(layer, out var current);
            var hasTransition = modelData.TryGetTransition(current, target, out var transition);

            if (!hasCurrent || !hasTransition) return false;
            
            entryBuffer = state.SetAnimation(layer, transition, false);

            if (callback != null) entryBuffer.Complete += _ => callback.Invoke();
                
            state.AddAnimation(layer, target, loop, 0f);
            
            return true;
        }

        private bool TryGetCurrentAnimation(int layer, out SpineAnimation result)
        {
            result = state.GetCurrent(layer)?.Animation;
    
            return result is not null;
        }
        
        private void EventHandler(TrackEntry trackEntry, Event e)
        {
            switch (e.Data.Name)
            {
                case "attackHit" : OnHit.Invoke(); break;
                case "skillHit" : OnHit.Invoke(); break;
                case "holdingHit":
                {
                    OnHit.Invoke(); break; 
                }
                case "footstep" : break;
                default:
                {
                    Debug.LogWarning($"Unknown animation event key in. input:{e.Data.Name}");
                    break;
                }
            }
        }

        private void Awake()
        {
            skeletonAnimation ??= GetComponent<SkeletonAnimation>();
            state             =   skeletonAnimation.AnimationState;
        }

        private void OnEnable() => skeletonAnimation.AnimationState.Event += EventHandler;
        private void OnDisable() => skeletonAnimation.AnimationState.Event -= EventHandler;
        
#if UNITY_EDITOR
        [SpineEvent(dataField : "skAnimation", fallbackToTextField = true)]
        [SerializeField] private string eventNameList;
#endif
    }
}
