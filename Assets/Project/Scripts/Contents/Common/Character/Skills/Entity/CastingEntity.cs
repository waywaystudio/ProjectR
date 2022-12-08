using System;
using System.Collections;
using Common.Character.Skills.Core;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Skills.Entity
{
    [Serializable]
    public class CastingEntity : BaseEntity
    {
        private bool onCasting;
        
        [ShowInInspector] private float remainCastingTime;

        public override bool IsReady => !onCasting;
        
        private float CastingTime { get; set; }
        private float CastingTick { get; set; }
        private float RemainCastingTime
        {
            get => remainCastingTime; 
            set => remainCastingTime = Mathf.Max(0, value);
        }
        
        public override void OnRegistered()
        {
            Skill.OnStarted += StartCasting;
            Skill.OnInterrupted += BreakCasting;
            Skill.OnCompleted += ResetTick;
        }

        public override void OnUnregistered()
        {
            Skill.OnStarted -= StartCasting;
            Skill.OnInterrupted -= BreakCasting;
            Skill.OnCompleted -= ResetTick;
        }


        protected virtual void SetEntity()
        {
            CastingTime = SkillData.CastingTime;
        }
        
        private void StartCasting() => StartCoroutine(Casting());
        private void BreakCasting()
        {
            onCasting = false;
            RemainCastingTime = CastingTime;
            
            StopAllCoroutines();
        }
        
        private IEnumerator Casting()
        {
            onCasting.OnTrue(() =>
            {
                Debug.Log("Casting Interrupted");
                Skill.InterruptedSkill();
            });
            
            onCasting = true;
            RemainCastingTime = CastingTime;
            
            while (RemainCastingTime > 0f)
            {
                RemainCastingTime -= CastingTick;
                yield return null;
            }
            
            onCasting = false;
            Skill.CompleteSkill();
        }
        
        private void ResetTick() => RemainCastingTime = CastingTime;

        protected override void Awake()
        {
            base.Awake();
            
            CastingTick = Time.deltaTime;
        }


#if UNITY_EDITOR
        protected override void OnEditorInitialize()
        {
            flag = EntityType.Casting;
            
            SetEntity();
        }
#endif
    }
}
