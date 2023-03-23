using System;
using DG.Tweening;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class Processor : Observable<float>
    {
        private const float Tolerance = 0.00001f;
        private float endTime;
        private float reciprocal;
        private Tween tween;

        public bool IsEnded { get; private set; } = true;
        public float NormalValue => Mathf.Clamp01(Value * reciprocal);

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
        public Processor(float endTime) { this.endTime = endTime; }
       

        public void Start()
        {
            tween?.Kill();
            tween = DOTween.To(() => Value, value => Value = value, endTime, Mathf.Abs(endTime - Value))
                           .SetEase(Ease.Linear)
                           .OnUpdate(() => OnValueChanged.Invoke(Value));
        }

        public void Stop()
        {
            tween?.Kill();
        }
    }
}
