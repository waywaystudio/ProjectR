using System;
using DG.Tweening;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class Processor : Observable<float>, ISequence
    {
        private const float Tolerance = 0.00001f;
        
        [SerializeField] private float endTime;

        private float reciprocal;
        private Tween tween;

        public bool IsEnded { get; private set; } = true;
        public float NormalValue => Mathf.Clamp01(Value * reciprocal);
        
        public ActionTable OnActivated { get; } = new();
        public ActionTable OnCanceled { get; } = new();
        public ActionTable OnCompleted { get; } = new();
        public ActionTable OnEnded { get; } = new();
        
        public override float Value
        {
            get => value;
            set
            {
                if (Math.Abs(this.value - value) < Tolerance) return;

                this.value = value;
                OnValueChanged.Invoke(this.value);
            }
        }

        public float EndTime
        {
            get => endTime;
            set
            {
                if (Value > endTime)
                {
                    Value = 0f;
                    tween?.Kill();
                }

                endTime    = value;
                reciprocal = 1.0f / endTime;
            }
        }

        public Processor() : this(0f) { }
        public Processor(float endTime) : this(endTime, string.Empty, null) { }
        public Processor(float endTime, string completeKey, Action onComplete)
        {
            this.endTime = endTime;
            
            OnCompleted.Register(completeKey, onComplete);
        }

        public void Activate() => Activate(endTime);
        public void Activate(float endTime)
        {
            IsEnded = false;

            EndTime = endTime;
            Value   = 0.0f;
            
            OnActivated.Invoke();
            Start();
        }

        public void Cancel()
        {
            OnCanceled.Invoke();
            End();
        }

        private void Complete()
        {
            Value = 0f;
            
            tween?.Kill();
            OnCompleted.Invoke();
            
            End();
        }

        private void End()
        {
            IsEnded = true;
            
            OnEnded.Invoke();
        }

        private void Start()
        {
            tween?.Kill();
            tween = DOTween.To(() => Value, value => Value = value, endTime, Mathf.Abs(endTime - Value))
                           .SetEase(Ease.Linear)
                           .OnUpdate(() => OnValueChanged.Invoke(Value))
                           .OnComplete(Complete);
        }
    }
}
