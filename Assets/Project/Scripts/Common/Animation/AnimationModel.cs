using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Common.Animation
{
    using SpineAnimation = Spine.Animation;
    using SpineState = Spine.AnimationState;
    using Event = Spine.Event;
    
    public class AnimationModel : MonoBehaviour, IEditable
    {
        [SerializeField] protected AnimationModelData modelData;
        [SerializeField] private SkeletonAnimation skeletonAnimation;
        
        protected SpineState State;
        private Action onEntireAnimationComplete;
        private Action<TrackEntry> completeActionBuffer;
        private TrackEntry currentEntry;

        public ActionTable OnHit { get; } = new();
        private SpineAnimation TargetAnimation { get; set; }
    
        public virtual void Idle() { State.TimeScale = 1f; PlayLoop("idle"); }
        public virtual void Run() => PlayLoop("run");
        public virtual void Dead(Action callback = null) => PlayOnce("dead", 1f, callback);
        public virtual void Stun() => PlayLoop("stun");
        public virtual void Hit() => PlayLoop("hit");
        
        public void PlayOnce(string animationKey, float timeScale = 1f, Action callback = null) => Play(animationKey, 0, false, timeScale, callback);
        public void PlayLoop(string animationKey, float timeScale = 1f, Action callback = null) => Play(animationKey, 0, true, timeScale, callback);
        

        public void Play(string animationKey, int layer, bool loop, float timeScale, Action callback)
        {
            if (!modelData.TryGetAnimation(animationKey, out var target))
            {
                Debug.LogError($"Not Exist Animation Key {animationKey}");
                return;
            }

            if (currentEntry != null) Canceled();
            if (callback != null)
            {
                completeActionBuffer =  null;
                completeActionBuffer += _ => callback.Invoke();
            }
            
            if (IsSameAnimation(target, layer, loop)) return;
            if (HasTransition(target, layer, loop, callback)) return;

            currentEntry           = State.SetAnimation(layer, target, loop);
            currentEntry.TimeScale = timeScale;

            // Handle Callback
            if (callback != null)
            {
                currentEntry.Complete += completeActionBuffer.Invoke;   
            }

            TargetAnimation = target;
        }

        public void Flip(Vector3 direction)
        {
            skeletonAnimation.Skeleton.ScaleX = direction.x switch
            {
                // 정남북에서 덜덜이 방지
                < -0.05f => -1.0f,
                > 0.05f => 1.0f,
                _ => skeletonAnimation.Skeleton.ScaleX
            };
        }
        
        
        private void Canceled()
        {
            if (currentEntry == null || completeActionBuffer == null) return;

            currentEntry.Complete -= completeActionBuffer.Invoke;
        }

        protected void EventHandler(TrackEntry trackEntry, Event e)
        {
            switch (e.Data.Name)
            {
                case "attack" :     OnHit.Invoke(); return;
                case "footstep" :   return;
                case "hit" :        return;
                case "get_hit" :    return;
                case "OnHit" : OnHit.Invoke(); return;
                default:
                {
                    Debug.LogWarning($"Unknown animation event key in. input:{e.Data.Name}");
                    break;
                }
            }
        }

        private bool IsSameAnimation(SpineAnimation target, int layer, bool loop)
        {
            if (!target.Equals(TargetAnimation) || !loop) return false;
            
            currentEntry = State.GetCurrent(layer);
        
            return true;
        }

        private bool HasTransition(SpineAnimation target, int layer, bool loop, Action callback)
        {
            var hasCurrent = TryGetCurrentAnimation(layer, out var current);
            var hasTransition = modelData.TryGetTransition(current, target, out var transition);
        
            if (!hasCurrent || !hasTransition) return false;
            
            currentEntry = State.SetAnimation(layer, transition, false);
        
            if (callback != null)
            {
                currentEntry.Complete += _ => callback.Invoke();
            } 
                
            State.AddAnimation(layer, target, loop, 0f);
            
            return true;
        }
        
        private bool TryGetCurrentAnimation(int layer, out SpineAnimation result)
        {
            result = State.GetCurrent(layer)?.Animation;
        
            return result is not null;
        }

        private void Awake()
        {
            skeletonAnimation ??= GetComponent<SkeletonAnimation>();
            State             =   skeletonAnimation.AnimationState;
        }

        private void OnEnable() => skeletonAnimation.AnimationState.Event += EventHandler;
        private void OnDisable() => skeletonAnimation.AnimationState.Event -= EventHandler;
        
        
#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            TryGetComponent(out skeletonAnimation);
        }
#endif
    }
}

/* Annotation
 * Reverse : CurrentEntry.Reverse = true
 * 뭔가 이상한 부분이 많아서 사용 금지
 */