using System;
using Core;
using MainGame;
using Spine;
using Spine.Unity;
using UnityEngine;
using Animation = Spine.Animation;
using AnimationState = Spine.AnimationState;

namespace Common.Character.Graphic
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

        public Animation TargetAnimation { get; private set; }
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        
        
        public void Idle() => Play("idle");
        public void Skill(string skillName, Action callback)
        {
            var skillData = MainData.GetSkill(skillName.ToEnum<IDCode>());
            var fixedTime = skillData.CastingTime;
            var animationKey = skillData.AnimationKey;
            
            // Assign Haste 
            var animationHasteValue = CharacterUtility.GetInverseHasteValue(Cb.StatTable.Haste);
            var inverse = CharacterUtility.GetHasteValue(Cb.StatTable.Haste);
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
            
            // TODO. 현재 Animation A에서 B로 넘어갈때 트랜지션 Animation이 따로 있는 경우가 없다.
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
        
        private void Flip() => Flip(Cb.Direction.Invoke());
        private void Flip(Vector3 direction)
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

        private bool TryGetCurrentAnimation(int layer, out Animation result)
        {
            result = state.GetCurrent(layer)?.Animation;
    
            return result is not null;
        }

        private void Awake()
        {
            TryGetComponent(out skeletonAnimation);
            
            state = skeletonAnimation.AnimationState;
            instanceID = GetInstanceID();
        }

        private void OnEnable()
        {
            Cb.OnIdle.Register(instanceID, Idle);
            Cb.OnWalk.Register(instanceID, Walk);
            Cb.OnRun.Register(instanceID, Run);
            Cb.OnSkill.Register(instanceID, Skill);
            Cb.OnUpdate.Register(instanceID, Flip);
        }

        private void OnDisable()
        {
            Cb.OnIdle.Unregister(instanceID);
            Cb.OnWalk.Unregister(instanceID);
            Cb.OnRun.Unregister(instanceID);
            Cb.OnSkill.Unregister(instanceID);
            Cb.OnUpdate.Unregister(instanceID);
        }
    }
}