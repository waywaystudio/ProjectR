using System;
using System.Collections;
using UnityEngine;

namespace Common.Character.Skills.Entity
{
    [Serializable]
    public class CastingEntity : EntityAttribution, IReadyRequired
    {
        private enum CastingType { Casting, Channeling }
        private CastingType castingType = CastingType.Casting;
        
        private bool onCasting;
        private float remainCastingTime;

        public bool IsReady => !onCasting;
        public float CastingTime { get; set; }
        public Action OnStart { get; set; }
        public Action OnBroken { get; set; }
        public Action OnCompleted { get; set; }
        
        private float CastingTick { get; set; }
        private float RemainCastingTime
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

        public void SetEntity()
        {
            CastingTime = SkillData.CastingTime;
            castingType = SkillData.AnimationKey == "casting"
                ? CastingType.Casting
                : CastingType.Channeling;
        }

        private void Awake()
        {
            CastingTick = Time.deltaTime;
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
            Flag = EntityType.Casting;
            
            SetEntity();
        }
#endif
    }
}
