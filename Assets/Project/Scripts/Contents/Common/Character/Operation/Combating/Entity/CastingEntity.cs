using System;
using System.Collections;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Operation.Combating.Entity
{
    [Serializable]
    public class CastingEntity : BaseEntity
    {
        private bool onCasting;
        private float remainTimer;
        [ShowInInspector]
        private float CastingTime { get; set; }
        private float CastingTick { get; set; }
        private float RemainTimer
        {
            get => remainTimer; 
            set => remainTimer = Mathf.Max(0, value);
        }
        
        public override bool IsReady => !onCasting;

        private void StartCasting() => StartCoroutine(Casting());
        private void BreakCasting()
        {
            onCasting = false;
            
            ResetRemainTimer();
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

            while (RemainTimer > 0f)
            {
                RemainTimer -= CastingTick;
                yield return null;
            }
            
            onCasting = false;
            
            ResetRemainTimer();
            Skill.CompleteSkill();
        }
        
        private void ResetRemainTimer() => RemainTimer = CastingTime;

        protected override void Awake()
        {
            base.Awake();

            CastingTime = SkillData.CastingTime;
            RemainTimer = CastingTime;
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

        private void Reset()
        {
            flag = EntityType.Casting;
        }
    }
}
