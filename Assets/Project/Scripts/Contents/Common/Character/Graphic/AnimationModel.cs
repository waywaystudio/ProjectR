using System;
using MainGame;
using Spine;
using Spine.Unity;
using UnityEngine;
using Animation = Spine.Animation;

namespace Common.Character.Graphic
{
    public class AnimationModel : MonoBehaviour
    {
        [SerializeField] private AnimationModelData modelData;

        private CharacterBehaviour cb;
        private SkeletonAnimation skeletonAnimation;
        private Spine.AnimationState state;
        private Action actionBuffer;
        private int instanceID;

        public Animation TargetAnimation { get; private set; }
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();


        public void LookLeft() => skeletonAnimation.Skeleton.ScaleX = 1.0f;
        public void LookRight() => skeletonAnimation.Skeleton.ScaleX = -1.0f;

        // Preset :: Do What;
        public void Idle() => Play("idle");
        public void Attack(string skillName, Action callback)
        {
            var skillData = MainData.GetSkillData(skillName);
            var fixedTime = skillData.CastingTime;

            actionBuffer = null;
            actionBuffer += callback;
            actionBuffer += Idle;

            Play("attack", 0, false, fixedTime, actionBuffer);
        }

        public void Skill(string skillName, Action callback)
        {
            var skillData = MainData.GetSkillData(skillName);
            var fixedTime = skillData.CastingTime;
            
            actionBuffer = null;
            actionBuffer += callback;
            actionBuffer += Idle;
            
            Play("skill", 0, false, fixedTime, actionBuffer);
        }
        
        public void Channeling(string skillName, Action callback)
        {
            var skillData = MainData.GetSkillData(skillName);
            var fixedTime = skillData.CastingTime;

            actionBuffer = null;
            actionBuffer += callback;
            actionBuffer += Idle;
            
            Play("channeling", 0, false, fixedTime, actionBuffer);
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

        public void Play(string animationKey, int layer = 0, bool loop = true, float fixedTime = 0f, Action callback = null)
        {
            if (!modelData.TryGetAnimation(animationKey, out var target))
            {
                Debug.LogError($"Not Exist Animation Key {animationKey}");
                return;
            }
            
            Play(target, layer, loop, fixedTime, callback);
        }

        private void Play(Animation target, int layer, bool loop, float speed = 0f, Action callback = null)
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
            
            if (speed != 0f)
            {
                var originalDuration = target.Duration;
                var toStaticValue = originalDuration / speed;
                var inverseValue = 1.0f / toStaticValue;

                state.TimeScale *= toStaticValue;
                entry.Complete += _ => state.TimeScale *= inverseValue;
            }

            if (callback != null) 
                entry.Complete += _ => callback.Invoke();

            TargetAnimation = target;
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
            Cb.OnAttack.Register(instanceID, Attack);
            Cb.OnSkill.Register(instanceID, Skill);
            Cb.OnChanneling.Register(instanceID, Channeling);
            
            Cb.OnLookLeft.Register(instanceID, LookLeft);
            Cb.OnLookRight.Register(instanceID, LookRight);
        }

        private void OnDisable()
        {
            Cb.OnIdle.UnRegister(instanceID);
            Cb.OnWalk.UnRegister(instanceID);
            Cb.OnRun.UnRegister(instanceID);
            Cb.OnAttack.UnRegister(instanceID);
            Cb.OnSkill.UnRegister(instanceID);
            Cb.OnChanneling.UnRegister(instanceID);
            
            Cb.OnLookLeft.UnRegister(instanceID);
            Cb.OnLookRight.UnRegister(instanceID);
        }
    }
}