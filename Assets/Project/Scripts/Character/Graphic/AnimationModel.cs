using System;
using Character.Combat.Skill;
using Core;
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
        // TODO. delete target
        [SerializeField] private CharacterBehaviour cb;
        [SerializeField] private AnimationModelData modelData;
        [SerializeField] private SkeletonAnimation skeletonAnimation;
        
        private SpineState state;
        private int instanceID;
        private Action callbackBuffer;
        private TrackEntry entryBuffer;
        private IPathfinding pathfindingEngine;
        
        public ActionTable OnHit { get; } = new();
        private SpineAnimation TargetAnimation { get; set; }
        
        /* Animation Preset */
        public void Idle()
        {
            state.TimeScale = 1f;
            
            if (!cb.DynamicStatEntry.IsAlive.Value) return;
            
            Play("idle");
        }
        public void Walk() => Play("walk");
        public void Run() => Play("run");
        public void Dead() => Play("Dead");

        // TODO. delete this function.
        public void Skill(SkillObject skill)
        {
            // var di = DataIndex.Corruption;
            // var skillData = MainData.GetSkill(di);
            // var _fixedTime = skillData.CastingTime;
            // var _animationKey = skillData.AnimationKey;
            
            var fixedTime = skill.FixedCastingTime;
            var animationKey = skill.AnimationKey;
            
            // Assign Haste 
            var animationHasteValue = CharacterUtility.GetInverseHasteValue(cb.StatTable.Haste);
            var inverse = CharacterUtility.GetHasteValue(cb.StatTable.Haste);
            state.TimeScale *= animationHasteValue;
            
            callbackBuffer =  null;
            callbackBuffer += cb.CompleteSkill;
            callbackBuffer += Idle;
            callbackBuffer += () => state.TimeScale *= inverse;

            Flip();
            Play(animationKey, 0, false, fixedTime, callbackBuffer);
        }

        public void Play(string animationKey, int layer = 0, bool loop = true, float timeScale = 0f, Action callback = null)
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
        
        public void Flip() => Flip(pathfindingEngine.Direction);
        
        private void Flip(Vector3 direction)
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
                case "channelingHit":
                {
                    Debug.Log("channelingHit Event In");
                    OnHit.Invoke(); break; 
                }
            }
        }

        private void Awake()
        {
            cb = GetComponentInParent<CharacterBehaviour>();
            instanceID = GetInstanceID();
            
            TryGetComponent(out skeletonAnimation);
            state = skeletonAnimation.AnimationState;
        }

        private void OnEnable()
        {
            skeletonAnimation.AnimationState.Event += EventHandler;
            
            cb.OnIdle.Register(instanceID, Idle);
            cb.OnWalk.Register(instanceID, Walk);
            cb.OnRun.Register(instanceID, Run);
            cb.OnDead.Register(instanceID, Dead);
            cb.OnUseSkill.Register(instanceID, Skill);
            cb.OnUpdate.Register(instanceID, Flip);
        }

        private void Start()
        {
            pathfindingEngine = cb.PathfindingEngine;
        }

        private void OnDisable()
        {
            skeletonAnimation.AnimationState.Event -= EventHandler;
        }
        
#if UNITY_EDITOR
        [SpineEvent(dataField : "skAnimation", fallbackToTextField = true)]
        [SerializeField] private string eventNameList;
#endif
    }
}