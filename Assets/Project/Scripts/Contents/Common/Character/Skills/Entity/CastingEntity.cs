using System;
using System.Collections;
using Common.Character.Skills.Core;
using UnityEngine;

namespace Common.Character.Skills.Entity
{
    [Serializable]
    public class CastingEntity : EntityAttribution
    {
        private enum CastingType { Casting, Channeling }
        private CastingType castingType = CastingType.Casting;
        
        private bool onCasting;
        private float remainCastingTime;

        public override bool IsReady => !onCasting;
        public float CastingTime { get; set; }
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
            RemainCastingTime = CastingTime;
            
            StopAllCoroutines();
        }

        protected override void SetEntity()
        {
            CastingTime = SkillData.CastingTime;
            castingType = SkillData.AnimationKey == "casting"
                        ? CastingType.Casting
                        : CastingType.Channeling;
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
            Skill.OnCompleted?.Invoke();

            yield return null;
        }

        protected override void Awake()
        {
            base.Awake();
            
            CastingTick = Time.deltaTime;
        }

        private void OnEnable()
        {
            Skill.OnStarted += StartCasting;
            Skill.OnInterrupted += BreakCasting;
        }
        
        private void OnDisable()
        {
            Skill.OnStarted -= StartCasting;
            Skill.OnInterrupted -= BreakCasting;
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
