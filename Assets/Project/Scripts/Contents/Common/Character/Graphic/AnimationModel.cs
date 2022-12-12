using System;
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

        public Animation TargetAnimation { get; private set; }
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();


        public void LookLeft() => skeletonAnimation.Skeleton.ScaleX = 1.0f;
        public void LookRight() => skeletonAnimation.Skeleton.ScaleX = -1.0f;

        // Preset :: Do What;
        public void Idle() => Play("idle", 0);
        public void Attack() => Play("attack", 0, false, Idle);
        public void Skill() => Play("skill", 0, false, Idle);
        public void Walk(Vector3 fakeValue) => Play("walk", 0);
        public void Run(Vector3 fakeValue)=> Play("run", 0);
        // AttackVariant...
        // PowerAttack
        // Ultimate
        // Hit
        // Groggy
        // Stun
        // Dead
        // InstantSpell
        // CastingSpell
        // ChannelingSpell

        public void Play(string animationKey, int layer, bool loop = true, Action callback = null)
        {
            if (!modelData.TryGetAnimation(animationKey, out var target))
            {
                Debug.LogError($"Not Exist Animation Key {animationKey}");
                return;
            }
            
            Play(target, layer, loop, callback);
        }

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
        
        private void Play(Animation target, int layer, bool loop, float speed, Action callback = null)
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
    
        private bool TryGetCurrentAnimation(int layer, out Animation result)
        {
            result = state.GetCurrent(layer)?.Animation;
    
            return result is not null;
        }

        private float ToStaticPlayTime(Animation original, float to)
        {
            var result = 1.0f;

            if (to != 0.0f && original.Duration != 0.0f)
            {
                result = to / original.Duration;
            }

            return result;
        }
        
        private void Awake()
        {
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            state = skeletonAnimation.AnimationState;
        }

        private void OnEnable()
        {
            Cb.OnIdle += Idle;
            Cb.OnWalk += Walk;
            Cb.OnRun += Run;
            Cb.OnAttack += Attack;
            Cb.OnSkill += Skill;
            
            Cb.OnLookLeft += LookLeft;
            Cb.OnLookRight += LookRight;
        }

        private void OnDisable()
        {
            Cb.OnIdle -= Idle;
            Cb.OnWalk -= Walk;
            Cb.OnRun -= Run;
            Cb.OnAttack -= Attack;
            Cb.OnSkill -= Skill;
            
            Cb.OnLookLeft -= LookLeft;
            Cb.OnLookRight -= LookRight;
        }
    }
}