using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Animation
{
    public class CharacterAnimation : MonoBehaviour, IAnimator, IEditable
    {
        [SerializeField] private Animator animator;
        [SerializeField] private List<string> stateList;
        
        private static readonly int RunHash = Animator.StringToHash("Run");
        private static readonly int IdleHash = Animator.StringToHash("Idle");
        private static readonly int DieBackHash = Animator.StringToHash("DieBack");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int Charging1 = Animator.StringToHash("Charging");
        private Action Callback { get; set; }
        private float originalSpeed = 1f;
        private static readonly int WalkHash = Animator.StringToHash("Walk");

        public ActionTable OnHitEventTable { get; } = new();
        public string CurrentKey { get; private set; }

        [Button]
        public void Idle()
        {
            // if (CurrentKey == "Idle") return;

            animator.SetTrigger(IdleHash);
            CurrentKey     = "Idle";
            // animator.speed = originalSpeed;
            // Callback       = null;
        }

        public void Stop()
        {
            Idle();
            // Idle();
            // CurrentKey     = string.Empty;
            // animator.speed = originalSpeed;
            // Callback       = null;
        }

        [Button]
        public void Run()
        {
            if (CurrentKey == "Run") return;

            CurrentKey = "Run";
            animator.SetTrigger(WalkHash);
        }
        

        public void Stun() { }
        public void Hit() { }
        public void Play(string key) => Play(key, 1f, null);
        public void Play(string key, float multiplier, Action callback)
        {
            if (CurrentKey == "Stop")
            {
                animator.ResetTrigger(IdleHash);
                // Stop();
            }
            
            CurrentKey = key;
            animator.SetTrigger(Animator.StringToHash(key));

            originalSpeed  = animator.speed;
            animator.speed = originalSpeed * multiplier;
            Callback       = callback;
        }

        public void OnHit()
        {
            Debug.Log("OnHit In");
            OnHitEventTable.Invoke();
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
            Debug.Log("Animation Callback In");
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
                    time = clip.length * 0.9f, 
                    functionName = "AnimationCallback",
                };
            
                clip.AddEvent(callback);
            });
        }
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            
        }
#endif
    }
}
