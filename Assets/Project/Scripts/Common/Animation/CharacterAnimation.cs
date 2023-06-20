using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Animation
{
    public class CharacterAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private AnimationBridge bridge;
        
        private static readonly int RunHash = Animator.StringToHash("Run");
        private static readonly int IdleHash = Animator.StringToHash("Idle");
        private static readonly int DieBackHash = Animator.StringToHash("DieBack");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private Action Callback { get; set; }

        public ActionTable OnHitActionTable { get; } = new();
        public string CurrentKey { get; private set; }

        public void Stop() => Idle();
        [Button]
        public void Idle()
        {
            animator.SetTrigger(IdleHash);
            CurrentKey     = "Idle";
            animator.speed = 1f;
            Callback       = null;
        }

        [Button]
        public void Run()
        {
            CurrentKey = "Run";
            animator.SetTrigger(RunHash);
        }
        
        [Button]
        public void Dead()
        {
            CurrentKey = "Dead";
            animator.SetTrigger(DieBackHash);
        }

        [Button]
        public void Attack()
        {
            CurrentKey = "Attack";
            animator.SetTrigger(AttackHash);
        }

        public void Stun() { }
        public void Hit() { }
        public void Play(string key, float animatorSpeed, Action callback)
        {
            CurrentKey = key;
            animator.SetTrigger(AttackHash);
            
            animator.speed = animatorSpeed;
            Callback       = callback;
        }

        public void OnHit()
        {
            Debug.Log("OnHit In");
            OnHitActionTable.Invoke();
        }

        public void BridgeCallback1()
        {
            // Debug.Log("BridgeCallback1");
        }
        
        public void BridgeCallback2()
        {
            // Debug.Log("BridgeCallback2");
        }
        
        public void BridgeCallback3()
        {
            // Debug.Log("BridgeCallback3");
        }
        
        /* Register In Runtime */
        public void AnimationCallback()
        {
            Callback?.Invoke();
        }
        

        private void Awake()
        {
            // Add Callback Event to None Looping Animation
            animator.runtimeAnimatorController.animationClips.ForEach(clip =>
            {
                if (clip.isLooping) return;
                
                var callback = new AnimationEvent
                {
                    time = clip.length, 
                    functionName = "AnimationCallback",
                };

                clip.AddEvent(callback);
            });
        }
    }
}
