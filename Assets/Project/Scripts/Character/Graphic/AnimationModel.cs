using System;
using Core;
using MainGame;
using Spine;
using Spine.Unity;
using UnityEngine;
using Animation = Spine.Animation;
using AnimationState = Spine.AnimationState;

namespace Character.Graphic
{
    public class AnimationModel : MonoBehaviour
    {
        [SerializeField] private AnimationModelData modelData;

        private CharacterBehaviour cb;
        private SkeletonAnimation skeletonAnimation;
        private AnimationState state;
        private int instanceID;
        
        private Action callbackBuffer;
        private TrackEntry entryBuffer;
        private IPathfinding pathfindingEngine;

        private Animation TargetAnimation { get; set; }
        
        
        public void Idle() => Play("idle");
        public void Skill(string skillName, Action callback)
        {
            var skillData = MainData.GetSkill(skillName.ToEnum<DataIndex>());
            var fixedTime = skillData.CastingTime;
            var animationKey = skillData.AnimationKey;
            
            // Assign Haste 
            var animationHasteValue = CharacterUtility.GetInverseHasteValue(cb.StatTable.Haste);
            var inverse = CharacterUtility.GetHasteValue(cb.StatTable.Haste);
            state.TimeScale *= animationHasteValue;
            
            callbackBuffer = null;
            callbackBuffer += callback;
            callbackBuffer += Idle;
            callbackBuffer += () => state.TimeScale *= inverse;

            Flip();
            Play(animationKey, 0, false, fixedTime, callbackBuffer);
        }
        public void Walk() => Play("walk");
        public void Run()=> Play("run");
        // Attack Variants...
        // PowerAttack
        // Ultimate
        // Groggy
        // Stun
        // Dead
        // InstantSpell
        // CastingSpell
        

        private void Play(string animationKey, int layer = 0, bool loop = true, float fixedTime = 0f, Action callback = null)
        {
            if (!modelData.TryGetAnimation(animationKey, out var target))
            {
                Debug.LogError($"Not Exist Animation Key {animationKey}");
                return;
            }
            
            Play(target, layer, loop, fixedTime, callback);
        }

        private void Play(Animation target, int layer, bool loop, float customTime, Action callback)
        {
            entryBuffer = null;
            
            // if Target Animation is same with Current, and it is loopTime, 
            // but include callback.
            if (target.Equals(TargetAnimation) && loop)
            {
                entryBuffer = state.GetCurrent(layer);
                
                if (callback != null) 
                    entryBuffer.Complete += _ => callback.Invoke();
                
                return;
            }
            
            // TODO. ?????? Animation A?????? B??? ???????????? ???????????? Animation??? ?????? ?????? ????????? ??????.
            var hasCurrent = TryGetCurrentAnimation(layer, out var current);
            var hasTransition = modelData.TryGetTransition(current, target, out var transition);

            if (hasCurrent && hasTransition)
            {
                entryBuffer = state.SetAnimation(layer, transition, false);

                if (callback != null) 
                    entryBuffer.Complete += _ => callback.Invoke();
                
                state.AddAnimation(layer, target, loop, 0f);
                return;
            }

            entryBuffer = state.SetAnimation(layer, target, loop);

            // Assign Custom Fixed Animation Speed
            if (customTime != 0f)
            {
                var originalDuration = target.Duration;
                var toStaticValue = originalDuration / customTime;

                state.TimeScale *= toStaticValue;
                entryBuffer.Complete += _ => state.TimeScale /= toStaticValue;
            }

            // Handle Callback
            if (callback != null)
            {
                entryBuffer.Complete += _ => callback.Invoke();
            }

            TargetAnimation = target;
        }
        
        private void Flip() => Flip(pathfindingEngine.Direction);
        private void Flip(Vector3 direction)
        {
            // ???????????? x, z??? ???????????? ????????? ????????? ??????????????? ????????????.
            // ?????? ?????? ?????????, direction ??? x?????? ????????????.
            skeletonAnimation.Skeleton.ScaleX = direction.x switch
            {
                < 0 => -1.0f,
                > 0 => 1.0f,
                _ => skeletonAnimation.Skeleton.ScaleX
            };
        }

        private bool TryGetCurrentAnimation(int layer, out Animation result)
        {
            result = state.GetCurrent(layer)?.Animation;
    
            return result is not null;
        }

        private void Awake()
        {
            TryGetComponent(out skeletonAnimation);

            cb         = GetComponentInParent<CharacterBehaviour>();
            instanceID = GetInstanceID();
            state      = skeletonAnimation.AnimationState;
        }

        private void Start()
        {
            pathfindingEngine = cb.PathfindingEngine;
        }

        private void OnEnable()
        {
            cb.OnIdle.Register(instanceID, Idle);
            cb.OnWalk.Register(instanceID, Walk);
            cb.OnRun.Register(instanceID, Run);
            cb.OnSkill.Register(instanceID, Skill);
            cb.OnUpdate.Register(instanceID, Flip);
        }

        private void OnDisable()
        {
            cb.OnIdle.Unregister(instanceID);
            cb.OnWalk.Unregister(instanceID);
            cb.OnRun.Unregister(instanceID);
            cb.OnSkill.Unregister(instanceID);
            cb.OnUpdate.Unregister(instanceID);
        }
    }
}