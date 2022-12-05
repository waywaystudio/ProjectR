using System;
using System.Collections;
using UnityEngine;

namespace Common.Character.Skills.Entity
{
    [Serializable]
    public class CastingEntity : EntityAttribution, ICastingEntity, IReadyRequired
    {
        private enum CastingType { Casting, Channeling }
        
        [SerializeField] private float castingTime;
        [SerializeField] private CastingType castingType = CastingType.Casting;

        public bool IsReady => !onCasting;
        private bool onCasting;
        private float tick;
        private float remainCastingTime;

        public Action OnStart { get; set; }
        public Action OnBroken { get; set; }
        public Action OnCompleted { get; set; }
        
        public float CastingTime { get => castingTime; set => castingTime = value; }
        public float CastingTick { get => tick; set => tick = value; }
        public float RemainCastingTime
        {
            get => remainCastingTime; 
            set => remainCastingTime = Mathf.Max(0, value);
        }

        public void StartCasting() => StartCoroutine(Casting());
        public void BreakCasting()
        {
            onCasting = false;
            StopAllCoroutines();
        }

        private void Awake()
        {
            tick = Time.deltaTime;
        }

        private void OnEnable()
        {
            OnStart += StartCasting;
            OnBroken += BreakCasting;
        }
        
        private IEnumerator Casting()
        {
            onCasting = true;
            RemainCastingTime = CastingTime;
            
            while (RemainCastingTime > 0f)
            {
                RemainCastingTime -= CastingTick;
                yield return null;
            }
            
            onCasting = false;
            OnCompleted?.Invoke();

            yield return null;
        }

        private void OnDisable()
        {
            OnStart -= StartCasting;
            OnBroken -= BreakCasting;
        }

#if UNITY_EDITOR
        protected override void OnEditorInitialize()
        {
            base.OnEditorInitialize();
            
            Flag = EntityType.Casting;
            CastingTime = StaticData.CastingTime;
            castingType = StaticData.AnimationKey == "casting"
                ? CastingType.Casting
                : CastingType.Channeling;
        }
#endif
    }
}
